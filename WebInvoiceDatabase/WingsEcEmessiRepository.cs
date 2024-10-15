using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebInvoiceDatabase.Context;
using WebInvoiceDatabase.Models;

namespace WebInvoiceDatabase
{
    public interface IWingsEcEmessiRepository
    {
        Task<WingsECEmessi> InsertOrUpdateWingsECEmessiAsync(WingsECEmessi wingsECEmessi);
    }

    public class WingsEcEmessiRepository : BaseRepository, IWingsEcEmessiRepository
    {
        public WingsEcEmessiRepository(string connectionString) : base(connectionString) { }
        public async Task<WingsECEmessi> InsertOrUpdateWingsECEmessiAsync(WingsECEmessi wingsECEmessi)
        {
            if (wingsECEmessi == null)
                return null;

            WingsECEmessi retEC = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                bool isSaveChanges = false;

                WingsECEmessi dbECData = await entity.WingsECEmessis.Where(x => x.ECNumero == wingsECEmessi.ECNumero).SingleOrDefaultAsync();

                //Can't update dbECData.FlgDocManuale == false. EC data forc manually not managed on this function
                if (dbECData != null)
                {
                    if (dbECData.FlgDocManuale == false)
                    {
                        bool reprint = (dbECData.ECInviatoCliente == 1 || dbECData.ECArchiviato == 1) ? true : false;

                        dbECData.ECData = wingsECEmessi.ECData;
                        dbECData.ClienteCodice = wingsECEmessi.ClienteCodice;
                        dbECData.ClienteDescrizione = wingsECEmessi.ClienteDescrizione;
                        dbECData.DocumentoTipo = wingsECEmessi.DocumentoTipo;
                        dbECData.DocumentoDescrizione = wingsECEmessi.DocumentoDescrizione;
                        dbECData.AgenziaCwtCodice = wingsECEmessi.AgenziaCwtCodice;
                        dbECData.ECFilePath = wingsECEmessi.ECFilePath;
                        dbECData.ECFileName = wingsECEmessi.ECFileName;
                        dbECData.ImportazioneNumero = (short)((dbECData.ImportazioneNumero ?? 0) + 1);
                        dbECData.ECTesto = wingsECEmessi.ECTesto;
                        dbECData.ImpNettoIva = wingsECEmessi.ImpNettoIva;
                        dbECData.ImpIva = wingsECEmessi.ImpIva;
                        dbECData.ImpTotale = wingsECEmessi.ImpTotale;
                        dbECData.ImpPagato = wingsECEmessi.ImpPagato;
                        dbECData.ImpBollo = wingsECEmessi.ImpBollo;
                        dbECData.ImpSaldo = wingsECEmessi.ImpSaldo;
                        dbECData.ImportazioneDataOra = DateTime.Now;
                        dbECData.ECDataStampa = wingsECEmessi.ECDataStampa;
                        dbECData.NumeroPagine = wingsECEmessi.NumeroPagine;
                        dbECData.ECRistampato = reprint ? (short)1 : (short)0;

                        isSaveChanges = true;
                    }
                }
                else
                {
                    //Create
                    wingsECEmessi.ImportazioneNumero = 1;
                    wingsECEmessi.ImportazioneDataOra = DateTime.Now;
                    wingsECEmessi.ImportazioneLastDataOra = DateTime.Now;

                    entity.WingsECEmessis.Add(wingsECEmessi);
                    isSaveChanges = true;
                }

                int countSqlResponseItem = 0;
                if (isSaveChanges)
                    countSqlResponseItem = await entity.SaveChangesAsync();

                retEC = await entity.WingsECEmessis.Where(x => x.ECNumero == wingsECEmessi.ECNumero).SingleOrDefaultAsync();
            }

            return retEC;
        }
    }
}
