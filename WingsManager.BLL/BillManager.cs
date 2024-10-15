using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.DataStructureManager;
using UtilityTools.WrapperClasses;
using WingsManager.BLL.MailSender;
using WingsManager.DAL;
using WingsManager.Model.Configurations.BillManager;
using WingsManager.Model.Imports;
using WingsManager.Model.Managers.Bill;

namespace WingsManager.BLL
{
    public interface IBillManager
    {
        Task ImportWingsBillWorker(CancellationToken cancellationToken);
    }
    public class BillManager : IBillManager
    {
        private readonly IWingsService _wingsService = null;
        private readonly IBillService _billService = null;
        private readonly IGenesisService _genesisService = null;
        private readonly ConfigParameters _config = null;
        private readonly ISendEmail _sendMailService;
        private readonly ILogger<BillManager> _logger = null;

        public BillManager(ILogger<BillManager> logger, IWingsService wingsService, IBillService billService, IGenesisService genesisService,
                            ISendEmail sendMailService)
        {
            _wingsService = wingsService;
            _billService = billService;
            _genesisService = genesisService;
            _logger = logger;
            string ecConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManagerSettings", "BillSettings.json");
            _config = JsonManager.DeserializeObjectByFilePath<ConfigParameters>(ecConfigFile);
            _sendMailService = sendMailService;
        }

        public async Task ImportWingsBillWorker(CancellationToken cancellationToken)
        {
            this.CheckImportWingsBillWorker();

            //Get file from folder
            string[] fileNames = Directory.GetFiles(this._config.FoldersPath.Original,"*.XML");

            for (int idxFile = 0; idxFile < fileNames.Length; idxFile++)
            {
                string fileName = fileNames[idxFile];
                FileInfo fileInfo = new FileInfo(fileName);

                this._logger.LogInformation($"Working file '{fileName}'");

                try
                {
                    WingsXmlDocument wingsXmlDocument = await Common.GetWingsXmlDocumentByFile(fileName, cancellationToken);
                    if (wingsXmlDocument is null)
                        continue;

                    ImportDataWings importDataWings = await this.MappingWingsBillDocumentAsync(wingsXmlDocument, fileInfo, cancellationToken);
                    if (importDataWings == null)
                        throw new Exception($"Error while mapping xml document.");

                    #region Move file in destination directory
                    string destinationPath = null;
                    string fileCreationTime = fileInfo.CreationTime.ToString("yyyy-MM-dd");

                    switch (importDataWings.DocumentType)
                    {
                        case DocumentType.DebitNote:
                        case DocumentType.CreditNoteReport:
                            destinationPath = this._config.FoldersPath.CreditNote;//la cartella con la data per gli EC non va creata ci deve pensare il job degli EC// Path.Combine(this._config.FoldersPath.CreditNote, fileCreationTime);
                            break;
                        case DocumentType.CreditNoteReportMove://?
                        case DocumentType.InvoiceReport:
                            destinationPath = this._config.FoldersPath.InvoiceReport;//la cartella con la data per gli EC non va creata ci deve pensare il job degli EC//Path.Combine(this._config.FoldersPath.InvoiceReport, fileCreationTime);
                            break;
                        case DocumentType.Proforma:
                            destinationPath = Path.Combine(this._config.FoldersPath.ProformaAndOtherDocs, fileCreationTime);//da verificare
                            break;
                        case DocumentType.Bill:
                        case DocumentType.CreditNote:
                        //case DocumentType.CreditNoteReport: sono EC vanno inella casisticadebitnote
                        case DocumentType.BillEN://?
                        case DocumentType.Invoice:
                            destinationPath = Path.Combine(this._config.FoldersPath.Processed, fileCreationTime);
                            break;
                        case DocumentType.Unknown:
                        case DocumentType.CreditNoteReportInsert://?
                            destinationPath = Path.Combine(this._config.FoldersPath.OtherDocs, fileCreationTime);
                            break;
                        case DocumentType.Discarded:
                            destinationPath = Path.Combine(this._config.FoldersPath.Discarded, fileCreationTime);
                            break;
                        default:
                            destinationPath = Path.Combine(this._config.FoldersPath.Error, fileCreationTime);
                            break;
                    }

                    string destinationFile = Path.Combine(destinationPath, fileInfo.Name);
                    if (DirectoryWrapper.ExistsIfNotCreate(destinationFile) && !DirectoryWrapper.FileIsBusy(fileInfo))
                    {
                        File.Move(fileInfo.FullName, destinationFile,true);
                        this._logger.LogInformation($"Moved file '{fileName}' to '{destinationFile}'");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, $"An error occured during import file '{fileName}'. The file will be transferred to the errors folder and go to the next");
                    
                    string destinationFile = Path.Combine(this._config.FoldersPath.Error, fileInfo.CreationTime.ToString("yyyy-MM-dd"), Path.GetFileName(fileName));
                    if (DirectoryWrapper.ExistsIfNotCreate(destinationFile))
                        File.Move(fileName, destinationFile, true);
                    
                    this._logger.LogInformation($"Moved file '{fileName}' to '{destinationFile}'");
                    
                    string retEmail = await _sendMailService.SendMessage(subject: $"An error occured during import file '{fileName}' The file will be transferred to {destinationFile}",
                                                                            to: null, from: null,
                                                                            body: $"{ex.Message}{Environment.NewLine}{ex.StackTrace}", "");
                    
                    this._logger.LogInformation(retEmail);
                }
            }
        }
        private async Task<ImportDataWings> MappingWingsBillDocumentAsync(WingsXmlDocument wingsXmlDocument, FileInfo sourceFileInfo, CancellationToken cancellationToken)
        {
            if (wingsXmlDocument == null)
                return null;

            //Map data
            ImportDataWings importData = new ImportDataWings();

            importData.BillingDocument = WingsXmlDocument.GetBillingDocument(wingsXmlDocument.Text.FirstOrDefault());
            if (string.IsNullOrEmpty(importData.BillingDocument))
                throw new Exception("WingsXml discarded. Billing document is not formatted correctly or is empty");

            DocumentType documentType = DocumentType.Discarded;
            List<string> billingDocumentLines = importData.BillingDocument.Split("\n").ToList();

            importData.DocumentName = $"{billingDocumentLines.ElementAtOrDefault(0)?.Substring(0, 26).Trim()} {billingDocumentLines.ElementAtOrDefault(1)?.Substring(0, 26).Trim()}".Trim();
            
            #region Document name
            if (!string.IsNullOrEmpty(importData.DocumentName))
            {
                if (importData.DocumentName.Equals(this._config.Patterns.Bill_IT, StringComparison.InvariantCultureIgnoreCase)
                    || importData.DocumentName.Equals(this._config.Patterns.CreditBill_IT, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.Bill;
                else if (importData.DocumentName.Equals(this._config.Patterns.CreditNote_EN, StringComparison.InvariantCultureIgnoreCase)
                            || importData.DocumentName.Equals(this._config.Patterns.CreditNote_IT, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.CreditNote;
                //else if (importData.DocumentName.Equals(this._ecConfig.Patterns.CreditBill_IT, StringComparison.InvariantCultureIgnoreCase))//spostato in DocumentType.Bill
                //    documentType = DocumentType.CreditNote;
                else if (importData.DocumentName.Equals(this._config.Patterns.DebitNoteReport_IT, StringComparison.InvariantCultureIgnoreCase)
                            || importData.DocumentName.Equals(this._config.Patterns.DebitNoteReport_EN, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.DebitNote;
                else if (importData.DocumentName.Equals(this._config.Patterns.CreditNoteReport_IT, StringComparison.InvariantCultureIgnoreCase)
                            || importData.DocumentName.Equals(this._config.Patterns.CreditNoteReport_EN, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.CreditNoteReport;
                else if (importData.DocumentName.Equals(this._config.Patterns.Invoice_IT, StringComparison.InvariantCultureIgnoreCase)
                            || importData.DocumentName.Equals(this._config.Patterns.Invoice_EN, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.Invoice;
                else if (importData.DocumentName.Equals(this._config.Patterns.InvoiceReport_IT, StringComparison.InvariantCultureIgnoreCase)
                            || importData.DocumentName.Equals(this._config.Patterns.InvoiceReport_EN, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.InvoiceReport;
                else if (importData.DocumentName.Equals(this._config.Patterns.Proforma, StringComparison.InvariantCultureIgnoreCase))
                    documentType = DocumentType.Proforma;
                else
                    documentType = DocumentType.Unknown;
            }
            #endregion
            
            importData.DocumentType = documentType;

            switch (documentType)
            {
                case DocumentType.CreditNote:
                case DocumentType.Invoice:
                case DocumentType.BillEN:
                case DocumentType.Bill:
                    importData.CostCenter = new Regex(this._config.Patterns.CDC, RegexOptions.IgnoreCase).Match(importData.BillingDocument)?.Groups["value"]?.Value.Trim();

                    Match docNumberMatch = new Regex(this._config.Patterns.DocumentNumber).Match(wingsXmlDocument.MetaData.Number);
                    importData.CwtAgencyCode = int.Parse(docNumberMatch.Groups["cwtAgencyCode"]?.Value);

                    importData.DocumentNumber = int.Parse(docNumberMatch.Groups["documentNumber"]?.Value);
                    importData.DocumentIssueDate = WingsXmlDocument.NormalizeAndParseDate(wingsXmlDocument.MetaData.Date);

                    importData.DepartureDate = wingsXmlDocument.MetaData.SalesLine.Select(x => WingsXmlDocument.NormalizeAndParseDate(x.DepDate)).Where(x => x != null).Min(x => x.Value);
                    
                    //todo: questa parte di codice è cambiata, comunque compilo determinati campi se li trovo, come per esempio il codiceCliente e di conseguenza la descrizione del cliente
                    importData.ClientCode = int.Parse(new Regex(this._config.Patterns.CustomerNumber)
                                                            .Match(wingsXmlDocument.MetaData.Customer.CustomerNumber)
                                                            .Groups["code"]
                                                            .Value);
                    if (importData.ClientCode > 0)
                        importData.ClientName = await this._genesisService.GetCustomerNameByWingsCodeAsync(importData.ClientCode.ToString(), cancellationToken);

                    string[] wingsDocNumbers = wingsXmlDocument.MetaData?.SalesLine?.Select(x => x.DossierNumber.ToString()).Distinct().ToArray();
                    if (wingsDocNumbers?.Length > 1)
                        throw new Exception("An unexpected error has occurred. There are multiple numbers of reference wings in the document. The request failed to preserve its integrity");

                    if (wingsDocNumbers != null)
                        importData.DocumentWingsNumber = Convert.ToInt32(wingsDocNumbers[0]);

                    //Extract product name distinct in the sales lines
                    Regex regexHeader = new Regex(this._config.Patterns.HeaderPerformanceRow);
                    string test = billingDocumentLines.Where(x => regexHeader.IsMatch(x)).FirstOrDefault();
                    int idxHeaderLine = billingDocumentLines.FindIndex(x => regexHeader.IsMatch(x));
                    if (idxHeaderLine > 0)
                    {
                        idxHeaderLine = idxHeaderLine + 3;
                        importData.PassengerName = billingDocumentLines.Where(x => new Regex(this._config.Patterns.ServiceBoxPassenger).IsMatch(x))
                                                                .Select(x => new Regex(this._config.Patterns.ServiceBoxPassenger).Match(x).Groups["value"]?.Value)
                                                                ?.FirstOrDefault();
                        
                        importData.ArchivePath = this._config.FoldersPath.Processed;
                        importData.ArchiveFileName = sourceFileInfo.Name;

                        await this._billService.InsertOrUpdateBill(importData);

                    }
                    break;
                default:
                    break;
            }

            return importData;
        }

        private bool CheckImportWingsBillWorker()
        {
            return false;
        }
    }
}
