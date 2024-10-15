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
using WingsManager.DAL;
using WingsManager.Model.Configurations.VoucherManager;
using WingsManager.Model.Imports;
using WingsManager.Model.Managers.Entity;
using WingsManager.Model.Managers.Voucher;

namespace WingsManager.BLL
{
    public interface IVoucherMSFManager
    {
        //Task ImportVoucherWorker(CancellationToken cancellationToken);
    }
    public class VoucherMSFManager : IVoucherMSFManager
    {
        private readonly IWingsService _wingsService = null;
        private readonly IVoucherService _voucherService = null;
        private readonly IGenesisService _genesisService = null;
        private readonly IRepositoryService _repositoryService = null;
        private readonly ConfigParameters _ecConfig = null;
        private readonly ILogger<VoucherManager> _logger = null;

        public VoucherMSFManager(ILogger<VoucherManager> logger, IWingsService wingsService, IVoucherService voucherService, IGenesisService genesisService, IRepositoryService repositoryService)
        {
            this._wingsService = wingsService;
            this._voucherService = voucherService;
            this._genesisService = genesisService;
            this._repositoryService = repositoryService;
            this._logger = logger;
            string ecConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManagerSettings", "VoucherSettings.json");
            this._ecConfig = JsonManager.DeserializeObjectByFilePath<ConfigParameters>(ecConfigFile);
        }

        public async Task ImportVoucherWorker(CancellationToken cancellationToken)
        {
            int result = await this._repositoryService.MassiveUpdateVoucherStateWithoutSupplierAsync(cancellationToken);
            result = await this._repositoryService.MassiveUpdateVoucherOvernightStayPreviousAsync(cancellationToken);
        }
        //private ImportDataWings MappingWingsVoucherDocument(WingsXmlDocument wingsXmlDocument, FileInfo sourceFileInfo)
        //{
        //    //if (wingsXmlDocument == null)
        //    //    return null;

        //    ////Map data
        //    //ImportDataWings importData = new ImportDataWings();

        //    //importData.BillingDocument = WingsXmlDocument.GetBillingDocument(wingsXmlDocument.Text.FirstOrDefault());
        //    //if (string.IsNullOrEmpty(importData.BillingDocument))
        //    //    throw new Exception("WingsXml discarded. Billing document is not formatted correctly or is empty");

        //    //importData.CwtAgencyCode = Convert.ToInt32(new Regex(this._ecConfig.Patterns.AgencyCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim());
        //    //importData.ClientCode = Convert.ToInt32(new Regex(this._ecConfig.Patterns.ClientCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value?.Trim());
        //    //importData.DocumentIssueDate = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DocumentIssueDate, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yyyy");
        //    //importData.DocumentId = Convert.ToInt32(new Regex(this._ecConfig.Patterns.DocumentId, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim());
        //    //Match voucherMatch = new Regex(this._ecConfig.Patterns.VoucherNumber, RegexOptions.IgnoreCase).Match(importData.BillingDocument);
        //    //importData.VoucherNumber = voucherMatch.Groups["voucherNumber"]?.Value.Trim();
        //    //importData.VoucherSeries = voucherMatch.Groups["voucherSeries"]?.Value.Trim();            
        //    //importData.PassengerName = new Regex(this._ecConfig.Patterns.PassengerName, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
        //    //importData.VoucherType = new Regex(this._ecConfig.Patterns.VoucherType, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups[0]?.Value.Trim();
        //    //importData.SupplierDescription = new Regex(this._ecConfig.Patterns.SupplierDescription, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
        //    //importData.SupplierFax = new Regex(this._ecConfig.Patterns.SupplierFax_1, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
        //    //if (!string.IsNullOrEmpty(importData.SupplierFax))
        //    //{
        //    //    importData.SupplierFax = importData.SupplierFax.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
        //    //    if(!importData.SupplierFax.StartsWith("00"))
        //    //        importData.SupplierFax = $"00{importData.SupplierFax}";
        //    //}
        //    //else
        //    //{
        //    //    importData.SupplierFax = new Regex(this._ecConfig.Patterns.SupplierFax_2, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
        //    //    if (!string.IsNullOrEmpty(importData.SupplierFax))
        //    //    {
        //    //        importData.SupplierFax = importData.SupplierFax.Replace("+", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("/", string.Empty).Replace(" ", string.Empty);
        //    //        if (!importData.SupplierFax.StartsWith("00"))
        //    //            importData.SupplierFax = $"00{importData.SupplierFax}";
        //    //    }
        //    //}

        //    //importData.DataIn = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DataIn, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yy");
        //    //importData.DataOut = WingsXmlDocument.NormalizeAndParseDate(new Regex(this._ecConfig.Patterns.DataOut, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "dd/MM/yy");
        //    //importData.TotalAmount = WingsXmlDocument.NormalizeWingsAmount(new Regex(this._ecConfig.Patterns.TotalAmount, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim(), "it-IT");

        //    //string xmlValue = new Regex(this._ecConfig.Patterns.HarpCode, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();
        //    //string[] values = xmlValue.Split(" ");
        //    //int convertedValue = 0;
        //    //if (values.Length == 2)
        //    //{
        //    //    if (int.TryParse(values[0].Trim(), out convertedValue))
        //    //    {
        //    //        importData.HarpCode = $"{values[0].Trim()}{values[1].Trim()}";
        //    //    }
        //    //}
        //    //importData.WingsProductCode = new Regex(this._ecConfig.Patterns.WingsProduct, RegexOptions.IgnoreCase).Match(importData.BillingDocument).Groups["value"]?.Value.Trim();

        //    ////todo: This function i not understain why i need do it.
        //    //if (importData.BillingDocument.Contains(this._ecConfig.Patterns.Commission)
        //    //        || importData.BillingDocument.Contains(this._ecConfig.Patterns.Discount))
        //    //    importData.isVoucherCliente = false;
        //    //else
        //    //    importData.isVoucherCliente = true;

        //    //return importData;
        //}        
    }
}
