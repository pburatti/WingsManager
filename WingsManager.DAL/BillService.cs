using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebInvoiceDatabase;
using WebInvoiceDatabase.Models;
using WingsManager.Model.Configurations;
using WingsManager.Model.Managers.Bill;

namespace WingsManager.DAL
{
    public interface IBillService
    {
        Task<ImportDataWings> InsertOrUpdateBill(ImportDataWings importData);
    }
    public class BillService : ServiceBase, IBillService
    {
        public BillService(IOptions<AppConfiguration> appConfig) : base(appConfig) { }
        public async Task<ImportDataWings> InsertOrUpdateBill(ImportDataWings importData)
        {
            if (importData == null)
                return null;

            WingsBolleEmesseRepository wingsBolleEmesseRepository = new WingsBolleEmesseRepository(base.ConnectionString);

            WingsBolleEmesse wingsBolleEmesse = new WingsBolleEmesse();
            wingsBolleEmesse.AgenziaCWTCodice = importData.CwtAgencyCode;
            wingsBolleEmesse.OpeData = importData.DocumentIssueDate;
            wingsBolleEmesse.PartenzaData = importData.DepartureDate;
            wingsBolleEmesse.PNR = importData.GDSPnr ?? string.Empty;
            wingsBolleEmesse.PaxNome = importData.PassengerName ?? string.Empty;
            wingsBolleEmesse.ClienteCodice = importData.ClientCode;
            wingsBolleEmesse.ClRagSoc = importData.ClientName;
            wingsBolleEmesse.Bolla = importData.BillingDocument;
            wingsBolleEmesse.PraticaNumeroWings = importData.DocumentWingsNumber;
            wingsBolleEmesse.CentroDiCosto = importData.CostCenter.Substring(0, importData.CostCenter.Length > 20 ? 20 : importData.CostCenter.Length);//bug lunghezza dato; modificare su sql è troppo pesante: troppi dati 14/02/2022
            wingsBolleEmesse.DocumentoTipo = (short)importData.DocumentType;
            wingsBolleEmesse.AnnoFattura = importData.DocumentIssueDate?.Year;
            wingsBolleEmesse.NomeAllegati = importData.ArchivePath;
            wingsBolleEmesse.NomeFile = importData.ArchiveFileName;
            wingsBolleEmesse.Slip = importData.DocumentNumber;

            await wingsBolleEmesseRepository.InsertOrUpdateWingsBolleEmesseAsync(wingsBolleEmesse);

            return null;
        }
    }
}
