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
using WingsManager.Model.Configurations.AccountStatementManager;
using WingsManager.Model.Imports;
using WingsManager.Model.Managers.AccountStatement;

namespace WingsManager.BLL
{
    public interface IAccountStatementManager
    {
        Task ImportWingsAccountStatementWorker(CancellationToken cancellationToken);
        //Task<bool> ImportWingsAccountStatement(Stream xmlStream, FileInfo sourceFileInfo, CancellationToken cancellationToken);
    }
    public class AccountStatementManager : IAccountStatementManager
    {
        private readonly ConfigParameters _ecConfig = null;
        private readonly IAccountStatementService _accountStatementService = null;
        private readonly ISendEmail _sendMailService;
        private readonly ILogger<AccountStatementManager> _logger = null;

        public AccountStatementManager(ILogger<AccountStatementManager> logger, 
                                        IAccountStatementService accountStatementService,
                                        ISendEmail sendMailService)
        {
            string ecConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManagerSettings", "AccountStatementSettings.json");
            this._ecConfig = JsonManager.DeserializeObjectByFilePath<ConfigParameters>(ecConfigFile);
            this._logger = logger;
            this._accountStatementService = accountStatementService;
            _sendMailService = sendMailService;
        }

        public async Task ImportWingsAccountStatementWorker(CancellationToken cancellationToken)
        {
            this.CheckImportWingsAccountStatementWorker();

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

                    ImportDataWings importDataWings = this.MappingWingsAccountStatement(wingsXmlDocument, fileInfo);
                    if (importDataWings == null)
                        throw new Exception($"Error while mapping xml document.");

                    importDataWings = await this._accountStatementService.InsertOrUpdateAccountStatement(importDataWings);

                    #region Move file in destination directory
                    string destinationFile = null;
                    if (importDataWings == null)
                        throw new Exception($"An error occurred in '{fileName}' during database operation");

                    if (importDataWings.OutFlgManualDoc == false)
                    {
                        if (!string.IsNullOrEmpty(importDataWings.OutRestampPath))
                        {
                            if (File.Exists(importDataWings.OutRestampPath))
                            {
                                destinationFile = Path.Combine(this._ecConfig.FoldersPath.Reprint, fileInfo.CreationTime.ToString("yyyy-MM-dd"), Path.GetFileName(fileName));
                                if (DirectoryWrapper.ExistsIfNotCreate(destinationFile))
                                    File.Copy(importDataWings.OutRestampPath, destinationFile, true);
                            }
                            else
                            {
                                this._logger.LogWarning($"The file '{importDataWings.OutRestampPath}' is not found. It was not possible to a backup copy of the file to the folder '{Path.GetDirectoryName(destinationFile)}'");
                            }
                        }
                    }

                    destinationFile = Path.Combine(importDataWings.ArchivePath, Path.GetFileName(fileName));

                    if (DirectoryWrapper.ExistsIfNotCreate(destinationFile))
                        File.Move(fileName, destinationFile, true);

                    this._logger.LogInformation($"Moved file '{fileName}' to '{destinationFile}'");
                    #endregion
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

        private ImportDataWings MappingWingsAccountStatement(WingsXmlDocument wingsXmlDocument, FileInfo sourceFileInfo)
        {
            if (wingsXmlDocument == null)
                return null;
            
            //Map data
            string pageBreakReplacer = "@@@";
            ImportDataWings importData = new ImportDataWings();

            importData.CreationFileDate = sourceFileInfo?.CreationTime ?? DateTime.Now;
            //File details
            importData.ArchivePath = Path.Combine(this._ecConfig.FoldersPath.Processed, importData.CreationFileDate.Value.ToString("yyyy-MM-dd"));
            importData.ArchiveFileName = sourceFileInfo.Name;


            string clientCode = wingsXmlDocument.MetaData?.Customer?.CustomerNumber?.Trim();
            clientCode = clientCode.Substring(clientCode.Length - 7);
            if (!string.IsNullOrEmpty(clientCode))
                importData.ClientCode = Convert.ToInt32(clientCode);

            importData.ClientName = wingsXmlDocument.MetaData?.Customer?.CustomerName?.Trim();
            importData.DocumentNumber = wingsXmlDocument.MetaData?.Number?.Trim();

            importData.BillingDocument = WingsXmlDocument.GetBillingDocument(wingsXmlDocument.Text.FirstOrDefault());
            if (string.IsNullOrEmpty(importData.BillingDocument))
                throw new Exception("WingsXml discarded. Billing document is not formatted correctly or is empty");
            importData.DocumentPageCount = new Regex(pageBreakReplacer).Matches(importData.BillingDocument).Count + 1;

            #region DocumentType and language
            /*************add  2022-10-04 ***********************************************************/
            //it's retrieve the first line of document where surely there is the type (or part of the type) of document,
            //so it's exclude the possible other words compatible in the rest of document
            string DocumentName = importData.BillingDocument.Split("\n")[0];

            /****************************************************************************************/
            AmountClassType amountClassType = AmountClassType.None;
            //if (!string.IsNullOrEmpty(this._ecConfig.Patterns.DebitNoteIT) && new Regex(this._ecConfig.Patterns.DebitNoteIT).IsMatch(importData.BillingDocument))
            if (!string.IsNullOrEmpty(this._ecConfig.Patterns.DebitNoteIT) && new Regex(this._ecConfig.Patterns.DebitNoteIT).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.DebitNote;
                importData.DocumentLanguage = LanguageType.IT;
                amountClassType = AmountClassType.InvoiceReportIT;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.InvoiceReportIT) && new Regex(this._ecConfig.Patterns.InvoiceReportIT).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.InvoiceReportIT) && new Regex(this._ecConfig.Patterns.InvoiceReportIT).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.InvoiceReport;
                importData.DocumentLanguage = LanguageType.IT;
                amountClassType = AmountClassType.DebitNoteIT;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNoteReportIT) && new Regex(this._ecConfig.Patterns.CreditNoteReportIT).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNoteReportIT) && new Regex(this._ecConfig.Patterns.CreditNoteReportIT).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.CreditNoteReport;
                importData.DocumentLanguage = LanguageType.IT;
                amountClassType = AmountClassType.InvoiceReportIT;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNote2EN) && new Regex(this._ecConfig.Patterns.CreditNote2EN).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNote2EN) && new Regex(this._ecConfig.Patterns.CreditNote2EN).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.CreditNoteReport;
                importData.DocumentLanguage = LanguageType.EN;
                amountClassType = AmountClassType.InvoiceReportEN;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.DebitNoteEN) && new Regex(this._ecConfig.Patterns.DebitNoteEN).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.DebitNoteEN) && new Regex(this._ecConfig.Patterns.DebitNoteEN).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.DebitNote;
                importData.DocumentLanguage = LanguageType.EN;
                amountClassType = AmountClassType.InvoiceReportEN;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.InvoiceReportEN) && new Regex(this._ecConfig.Patterns.InvoiceReportEN).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.InvoiceReportEN) && new Regex(this._ecConfig.Patterns.InvoiceReportEN).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.InvoiceReport;
                importData.DocumentLanguage = LanguageType.EN;
                amountClassType = AmountClassType.DebitNoteEN;
            }
            //else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNoteEN) && new Regex(this._ecConfig.Patterns.CreditNoteEN).IsMatch(importData.BillingDocument))
            else if (!string.IsNullOrEmpty(this._ecConfig.Patterns.CreditNoteEN) && new Regex(this._ecConfig.Patterns.CreditNoteEN).IsMatch(DocumentName))
            {
                importData.DocumentType = DocumentType.CreditNoteReport;
                importData.DocumentLanguage = LanguageType.EN;
                amountClassType = AmountClassType.InvoiceReportEN;
            }
            #endregion

            #region Date of Document
            //todo: Document data, put in function
            string documentDatePattern = wingsXmlDocument.MetaData?.Date?.Format?.Trim();
            string documentDateElement = wingsXmlDocument.MetaData?.Date?.Value?.Trim();
            if (!string.IsNullOrEmpty(documentDatePattern))
                documentDatePattern = WingsXmlDocument.NormalizeWingsDatePattern(documentDatePattern);
            else
                documentDatePattern = importData.GetDefaultPatternDate();

            importData.DocumentDate = WingsXmlDocument.NormalizeAndParseDate(documentDateElement, documentDatePattern);
            #endregion

            #region Amounts
            AmountDataPattern amountPatterns = this._ecConfig.Patterns.GetAmountsPattern(amountClassType);
            if (amountPatterns != null)
            {
                importData.TotalAmounts = new WingsTotalAmounts();
                importData.TotalAmounts.VATNetAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.VATNetAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
                importData.TotalAmounts.VATAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.VATAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
                importData.TotalAmounts.TotalAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.TotalAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
                importData.TotalAmounts.PaidAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.PaidAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
                importData.TotalAmounts.StampAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.StampAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
                importData.TotalAmounts.BalanceAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(amountPatterns.BalanceAmount).Match(importData.BillingDocument).Groups["value"]?.Value, "it-IT");
            }
            #endregion

            importData.VATDetails = new List<WingsVATDetails>();
            //VAT Details
            foreach (var boxItem in wingsXmlDocument.MetaData.VATBoxWrapper)
            {
                WingsVATDetails vatDetail = new WingsVATDetails();
                vatDetail.VATCode = boxItem.VAT_CD;
                string vatPercentString = boxItem.VAT_PC;
                vatDetail.VATPercent = vatPercentString?.Substring(0, vatPercentString.Length - 1);
                vatDetail.Rateable = boxItem.VAT_BASE;
                vatDetail.VATAmount = boxItem.VAT_VAL;

                importData.VATDetails.Add(vatDetail);
            }

            return importData;
        }
        private bool CheckImportWingsAccountStatementWorker()
        {
            if (!Directory.Exists(this._ecConfig.FoldersPath.Original))
                throw new Exception("Original path not exists on not accessible");

            if (!Directory.Exists(this._ecConfig.FoldersPath.Reprint))
                throw new Exception("Reprint path not exists on not accessible");

            //Check document type config


            return true;
        }
    }
}