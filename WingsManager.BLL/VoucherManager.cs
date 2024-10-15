using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.DataStructureManager;
using UtilityTools.WrapperClasses;
using WingsManager.BLL.MailSender;
using WingsManager.DAL;
using WingsManager.Model.Configurations.VoucherManager;
using WingsManager.Model.Imports;
using WingsManager.Model.Managers.Entity;
using WingsManager.Model.Managers.Voucher;

namespace WingsManager.BLL
{
    public interface IVoucherManager
    {
        Task ImportVoucherWorker(CancellationToken cancellationToken);
    }
    public class VoucherManager : IVoucherManager
    {
        private readonly IWingsService _wingsService = null;
        private readonly IVoucherService _voucherService = null;
        private readonly IGenesisService _genesisService = null;
        private readonly IRepositoryService _repositoryService = null;
        private readonly ConfigParameters _ecConfig = null;
        private readonly ISendEmail _sendMailService;
        private readonly ILogger<VoucherManager> _logger = null;

        public VoucherManager(ILogger<VoucherManager> logger, IWingsService wingsService, IVoucherService voucherService, 
                                IGenesisService genesisService, IRepositoryService repositoryService,
                                ISendEmail sendMailService)
        {
            _wingsService = wingsService;
            _voucherService = voucherService;
            _genesisService = genesisService;
            _repositoryService = repositoryService;
            _logger = logger;
            string ecConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManagerSettings", "VoucherSettings.json");
            _ecConfig = JsonManager.DeserializeObjectByFilePath<ConfigParameters>(ecConfigFile);
            _sendMailService = sendMailService;
        }

        public async Task ImportVoucherWorker(CancellationToken cancellationToken)
        {
            this.CheckImportVoucherWorker();

            //Get file from folder
            string[] fileNames = Directory.GetFiles(this._ecConfig.FoldersPath.Original, "*.XML");

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

                    ImportDataWings importDataWings = this.MappingWingsVoucherDocument(wingsXmlDocument, fileInfo);
                    if (importDataWings == null)
                        throw new Exception($"Error while mapping xml document.");

                    if (importDataWings.ClientCode <= 0)
                        throw new Exception("Client code null or empty. Impossible to proceed GetSupplierByHarpCode()");


                    //Overwrite xml data with database
                    Client dbClientData = await this._repositoryService.GetClientByCodeAsync(importDataWings.ClientCode, cancellationToken);
                    if (dbClientData == null)
                        throw new Exception($"Unknown Client '{importDataWings.ClientCode}'");

                    importDataWings.SupplierDeliveryCode = dbClientData?.WingsDeliveryCode;
                    if (dbClientData.CwtAgencyCode == 69)
                        importDataWings.ClientName = dbClientData?.Name;

                    Supplier supplier = await this._repositoryService.GetSupplierByHarpCodeAsync(Convert.ToInt64(importDataWings.HarpCode), cancellationToken);
                    //if (supplier == null)
                    //    throw new Exception($"Unknown Supplier '{importDataWings.HarpCode}'");//perchè?

                    importDataWings.SupplierFax = supplier?.FaxNumber ?? importDataWings?.SupplierFax;
                    importDataWings.SupplierCountryCode = supplier?.CountryCode ?? importDataWings?.SupplierCountryCode;
                    importDataWings.SupplierEmailAddress = supplier?.EmailAddress??importDataWings?.SupplierEmailAddress;
                    //22/03/2022 questa riga stabilisce se il voucher va inviato per fax o per email: stabiliamo che è prioritaria COMUNQUE l'email VoucherSendType=2
                    //importDataWings.VoucherSendType = supplier?.SendVoucherType ?? 1; 
                    importDataWings.VoucherSendType = 2;

                    Product product = (await this._wingsService.GetProductsAsync(importDataWings.WingsProductCode, null, cancellationToken))?.LastOrDefault();
                    if (product == null)
                        throw new Exception($"Unknown Product '{importDataWings.WingsProductCode}'");

                    importDataWings.WingsProductCCCode = product?.CCCode;

                    importDataWings.ArchivePath = this._ecConfig.FoldersPath.Processed;
                    importDataWings.ArchiveFileName = Path.GetFileName(fileName);
                    int resultImportDatabase = await this._voucherService.InsertOrUpdateVoucherAsync(importDataWings, cancellationToken);
                    if(resultImportDatabase == 0)
                        throw new Exception($"During importing data in the database no error has occurred but no data operation has been made '{importDataWings.DocumentId}'");
                    //qui si stabilsce se il voucher deve essere inviato o solo ac
                    if (!importDataWings.FlgPaidCC || (importDataWings.FlgPaidCC && new List<string>() { "HOGIPO", "HONIPO" }.Contains(importDataWings.WingsProductCode)))
                    {
                        if (this._ecConfig.Patterns.VoucherSeriesType.Split('|').Contains(importDataWings.VoucherSeries))
                        {
                            //solo se sulla tabella AgenzieCwt InvioVoucherTipo==1 viene fatto l'invio automatico altrimenti sarà manuale da VoucherWeb2008
                            bool sendAutoByAgency = await this.IsSentAutomaticallyByAgencyAsync(importDataWings.CwtAgencyCode, importDataWings.VoucherSeries, importDataWings.SupplierCountryCode, cancellationToken);
                            if (sendAutoByAgency)
                            {
                                if (!importDataWings.isVoucherCliente && !importDataWings.VoucherType.Equals("PURCHASE HOTEL", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //if (importDataWings.VoucherSendType == 1)// la condizione di invio automatico dipende solo ed esclusivamente da sendAutoByAgency NON dal tipo di invio (email o fax è irrilevante)
                                    //{
                                    await SetAutomaticSentAsync(importDataWings, cancellationToken);//inserisce i dbo.InvioFaxEmail
                                    //}
                                }
                            }
                        }
                    }
                    string destinationFile = Path.Combine(this._ecConfig.FoldersPath.Processed, fileInfo.CreationTime.ToString("yyyy-MM-dd"), Path.GetFileName(fileName));
                    if (DirectoryWrapper.ExistsIfNotCreate(destinationFile) && !DirectoryWrapper.FileIsBusy(fileInfo))
                    {
                        File.Move(fileName, destinationFile, true);
                        this._logger.LogInformation( $"The Voucher has been correctly acquired and transferred to the archive '{destinationFile}'.");
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, $"An error occured during import file '{fileName}'. The file will be transferred to the errors folder and go to the next");

                    string destinationFile = Path.Combine(this._ecConfig.FoldersPath.Error, fileInfo.CreationTime.ToString("yyyy-MM-dd"), Path.GetFileName(fileName));
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
        private ImportDataWings MappingWingsVoucherDocument(WingsXmlDocument wingsXmlDocument, FileInfo sourceFileInfo)
        {
            if (wingsXmlDocument == null)
                return null;

            //Map data
            ImportDataWings importData = new ImportDataWings();

            importData.BillingDocument = WingsXmlDocument.GetBillingDocument(wingsXmlDocument.Text.FirstOrDefault());
            if (string.IsNullOrEmpty(importData.BillingDocument))
                throw new Exception("WingsXml discarded. Billing document is not formatted correctly or is empty");

            importData.CwtAgencyCode = Convert.ToInt32(new Regex(this._ecConfig.Patterns.AgencyCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim());
            importData.ClientCode = Convert.ToInt32(new Regex(this._ecConfig.Patterns.ClientCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value?.Trim());
            importData.DocumentIssueDate = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DocumentIssueDate, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yyyy");
            importData.DocumentId = Convert.ToInt32(new Regex(this._ecConfig.Patterns.DocumentId, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim());
            Match voucherMatch = new Regex(this._ecConfig.Patterns.VoucherNumber, RegexOptions.IgnoreCase).Match(importData.BillingDocument);
            importData.VoucherNumber = voucherMatch.Groups["voucherNumber"]?.Value.Trim();
            importData.VoucherSeries = voucherMatch.Groups["voucherSeries"]?.Value.Trim();            
            importData.PassengerName = new Regex(this._ecConfig.Patterns.PassengerName, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
            importData.VoucherType = new Regex(this._ecConfig.Patterns.VoucherType, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups[0]?.Value.Trim();
            importData.SupplierDescription = new Regex(this._ecConfig.Patterns.SupplierDescription, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
            importData.SupplierFax = new Regex(this._ecConfig.Patterns.SupplierFax_1, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
            if (!string.IsNullOrEmpty(importData.SupplierFax))
            {
                importData.SupplierFax = importData.SupplierFax.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
                if(!importData.SupplierFax.StartsWith("00"))
                    importData.SupplierFax = $"00{importData.SupplierFax}";
            }
            else
            {
                importData.SupplierFax = new Regex(this._ecConfig.Patterns.SupplierFax_2, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
                if (!string.IsNullOrEmpty(importData.SupplierFax))
                {
                    importData.SupplierFax = importData.SupplierFax.Replace("+", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty);
                    if (!importData.SupplierFax.StartsWith("00"))
                        importData.SupplierFax = $"00{importData.SupplierFax}";
                }
            }

            importData.DataIn = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DataIn, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yy");
            importData.DataOut = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DataOut, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yy");
            importData.TotalAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(this._ecConfig.Patterns.TotalAmount, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "it-IT");

            string xmlValue = new Regex(this._ecConfig.Patterns.HarpCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
            string[] values = xmlValue.Split(" ");
            int convertedValue = 0;
            if (values.Length == 2)
            {
                if (int.TryParse(values[0].Trim(), out convertedValue))
                {
                    importData.HarpCode = $"{values[0].Trim()}{values[1].Trim()}";
                }
            }
            importData.WingsProductCode = new Regex(this._ecConfig.Patterns.WingsProduct, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();

            //todo: This function i not understain why i need do it. 
            //.... ... because based on these two parameters it is established whether the Voucher text must go in the "VoucherCliente" field, and then it is a courtesy copy (CLIENT / PAYMENT COPY),          
            //rather than "VoucherFornitore" and then it is the original that the supplier needs
            if (importData.BillingDocument.Contains(this._ecConfig.Patterns.Commission)
                    || importData.BillingDocument.Contains(this._ecConfig.Patterns.Discount))
                importData.isVoucherCliente = false;
            else
                importData.isVoucherCliente = true;

            return importData;
        }

        private async Task<bool> IsSentAutomaticallyByAgencyAsync(int cwtAgencyCode, string voucherType,string SupplierCountryCode, CancellationToken cancellationToken)
        {
            Agency dbData = await _repositoryService.GetCwtAgencyAsync(cancellationToken, cwtAgencyCode, 6);//voucherType serve solo qui nella selct in VB6 il countryCwtCode (PaeseCwtCodice) è una costante e vale 6
            if (dbData != null)
            {
                //INC0809012 (2023-04-28): || voucherType.ToLower() == "3c" non vanno più inviati 
                //INC0839361 (2023-06-21) contrordine di  Varani ma solo per i fornitori italiani
                if ((voucherType == "29" || (voucherType.ToLower() == "3c" && SupplierCountryCode=="IT")) && (dbData.VoucherTypeSending.HasValue && dbData.VoucherTypeSending.Value == 1))
                    return true;
                else if (voucherType == "56" && (dbData.VoucherSr56Sending.HasValue && dbData.VoucherSr56Sending.Value == false))
                    return true;

            }

            return false;
        }

        //todo: Check this function
        private async Task<bool> SetAutomaticSentAsync(ImportDataWings importDataWings, CancellationToken cancellationToken)
        {
            EmailFaxSendData data = new EmailFaxSendData();
            data.ApplicationId = 1;
            data.DocumentId = importDataWings.VoucherIdInserted;
            data.EmailFax = importDataWings.VoucherSendType;
            data.EmailFaxDestination = importDataWings.VoucherSendType==1?importDataWings.SupplierFax: importDataWings.SupplierEmailAddress;
            data.ReceiverType = 1;//ma non c'è codifica da nessuna parte ...è così e basta
            data.SenderEmailAddress = importDataWings.VoucherSendType == 2 ? "no-reply@mycwt.it" : data.SenderEmailAddress;//inserire in vouchersetting o appsetting
            data.SenderName = "CWT Italia SRL";//inserire in vouchersetti o appsetting
            data.State = 0;//stato doi partenza
            data.StateDescription = "Richiesta automatica in corso";// solo se sulla tabella AgenzieCwt InvioVoucherTipo == 1 viene fatto l'invio automatico altrimenti sarà manuale da VoucherWeb2008
            data.SentDate = DateTime.Now;

            int countInserted = await this._repositoryService.InsertInvioFaxEmailAsync(data, cancellationToken);

            countInserted = await this._voucherService.SetInvioFaxEmailInviatiAsync(importDataWings.VoucherIdInserted, 1, 1, cancellationToken);
            return countInserted > 0;
        }
        private bool CheckImportVoucherWorker()
        {
            return false;
        }
    }
}
