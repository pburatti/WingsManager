using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebInvoiceDatabase;
using WebInvoiceDatabase.Models;
using WingsManager.Model.Configurations;
using WingsManager.Model.Managers.AccountStatement;

namespace WingsManager.DAL
{
    public interface IAccountStatementService
    {
        Task<ImportDataWings> InsertOrUpdateAccountStatement(ImportDataWings importData);
    }
    public class AccountStatementService : ServiceBase, IAccountStatementService
    {
        public AccountStatementService(IOptions<AppConfiguration> appConfig) : base(appConfig) { }
        public async Task<ImportDataWings> InsertOrUpdateAccountStatement(ImportDataWings importData)
        {
            //Import data in WingsEcEmessi and WingsEcEmessiIva
            WingsEcEmessiRepository wingsECEmessiRepository = new WingsEcEmessiRepository(base.ConnectionString);
            WingsEcEmessiIVARepository wingsECEmessiIVARepository = new WingsEcEmessiIVARepository(base.ConnectionString);

            //Mapping
            WingsECEmessi wingsEmessiDatabase = new WingsECEmessi();

            wingsEmessiDatabase.ClienteCodice = importData.ClientCode;
            wingsEmessiDatabase.ClienteDescrizione = importData.ClientName;
            wingsEmessiDatabase.AgenziaCwtCodice = importData.CwtAgencyCode;
            wingsEmessiDatabase.DocumentoTipo = (short)importData.DocumentType;
            wingsEmessiDatabase.DocumentoDescrizione = importData.GetDocumentDescription();
            wingsEmessiDatabase.ECNumero = importData.DocumentNumber;
            wingsEmessiDatabase.ECData = importData.DocumentDate;
            wingsEmessiDatabase.ECFilePath = importData.ArchivePath;
            wingsEmessiDatabase.ECFileName = importData.ArchiveFileName;
            wingsEmessiDatabase.ECTesto = importData.BillingDocument;
            wingsEmessiDatabase.ImpNettoIva = importData.TotalAmounts?.VATNetAmount ?? 0;
            wingsEmessiDatabase.ImpIva = importData.TotalAmounts?.VATAmount ?? 0;
            wingsEmessiDatabase.ImpTotale = importData.TotalAmounts?.TotalAmount ?? 0;
            wingsEmessiDatabase.ImpPagato = importData.TotalAmounts?.PaidAmount ?? 0;
            wingsEmessiDatabase.ImpBollo = importData.TotalAmounts?.StampAmount ?? 0;
            wingsEmessiDatabase.ImpSaldo = importData.TotalAmounts?.BalanceAmount ?? 0;
            wingsEmessiDatabase.ECDataStampa = importData.CreationFileDate;
            wingsEmessiDatabase.NumeroPagine = importData.DocumentPageCount;
            wingsEmessiDatabase.FlgDocManuale = false;

            wingsEmessiDatabase = await wingsECEmessiRepository.InsertOrUpdateWingsECEmessiAsync(wingsEmessiDatabase);

            if (wingsEmessiDatabase != null)
            {
                if (wingsEmessiDatabase.FlgDocManuale == false)
                {
                    await wingsECEmessiIVARepository.DeleteWingsEcEmessiIVAAsync(importData.DocumentNumber);

                    //Mapping
                    List<WingsEcEmessiIva> wingsEmessiIVADatabase = null;
                    wingsEmessiIVADatabase = importData.VATDetails.Select(x => new WingsEcEmessiIva()
                    {
                        CodiceIVA = x.VATCode,
                        AliquotaIVA = x.VATPercent,
                        ImportoImponibile = x.Rateable,
                        ImportoIVA = x.VATAmount,
                        EcNumero = importData.DocumentNumber
                    }).ToList();

                    await wingsECEmessiIVARepository.InsertWingsEcEmessiIVAAsync(wingsEmessiIVADatabase);
                }

                importData.OutFlgManualDoc = wingsEmessiDatabase.FlgDocManuale.Value;
                if (wingsEmessiDatabase.ImportazioneNumero > 1)
                    importData.OutRestampPath = Path.Combine(wingsEmessiDatabase.ECFilePath, wingsEmessiDatabase.ECFileName);

                return importData;
            }

            return null;
        }
    }
}
