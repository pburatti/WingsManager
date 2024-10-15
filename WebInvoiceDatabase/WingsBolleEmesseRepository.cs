using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoiceDatabase.Context;
using WebInvoiceDatabase.Models;

namespace WebInvoiceDatabase
{
    public interface IWingsBolleEmesseRepository
    {
        Task<List<WingsBolleEmesse>> GetWingsBolleEmesseAsync(decimal? codiceCliente = null, int? numeroPraticaWings = null, int? cwtAgencyCode = null, int? slip = null);
    }

    public class WingsBolleEmesseRepository : BaseRepository, IWingsBolleEmesseRepository
    {
        public WingsBolleEmesseRepository(string connectionString) : base(connectionString) { }
        public async Task<List<WingsBolleEmesse>> GetWingsBolleEmesseAsync(decimal? codiceCliente = null, int? numeroPraticaWings = null, int? cwtAgencyCode = null, int? slip = null)
        {
            List<WingsBolleEmesse> dbWingsBolleEmesse = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<WingsBolleEmesse> queryWingsBolleEmesse = entity.WingsBolleEmesses;

                if (codiceCliente.HasValue)
                    queryWingsBolleEmesse = queryWingsBolleEmesse.Where(x => x.ClienteCodice == codiceCliente.Value);
                if (numeroPraticaWings.HasValue)
                    queryWingsBolleEmesse = queryWingsBolleEmesse.Where(x => x.PraticaNumeroWings == numeroPraticaWings.Value);
                if (cwtAgencyCode.HasValue)
                    queryWingsBolleEmesse = queryWingsBolleEmesse.Where(x => x.AgenziaCWTCodice == cwtAgencyCode.Value);
                if (slip.HasValue)
                    queryWingsBolleEmesse = queryWingsBolleEmesse.Where(x => x.Slip == slip.Value);

                dbWingsBolleEmesse = await queryWingsBolleEmesse.ToListAsync();
            }

            return dbWingsBolleEmesse;
        }

        public async Task<WingsBolleEmesse> InsertOrUpdateWingsBolleEmesseAsync(WingsBolleEmesse wingsBolleEmesse)
        {
            if (wingsBolleEmesse == null)
                return null;

            WingsBolleEmesse retBE = null;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                WingsBolleEmesse dbBEData = await entity.WingsBolleEmesses.Where(x => x.Slip == wingsBolleEmesse.Slip 
                                                                                    && x.AgenziaCWTCodice == wingsBolleEmesse.AgenziaCWTCodice
                                                                                    && x.PraticaNumeroWings == wingsBolleEmesse.PraticaNumeroWings
                                                                                    && x.ClienteCodice == wingsBolleEmesse.ClienteCodice)
                                                                            .SingleOrDefaultAsync();
                
                if (dbBEData != null)
                {
                    dbBEData.OpeData = wingsBolleEmesse.OpeData;
                    dbBEData.PartenzaData = wingsBolleEmesse.PartenzaData;
                    dbBEData.PNR = wingsBolleEmesse.PNR;
                    dbBEData.PaxNome = wingsBolleEmesse.PaxNome;
                    dbBEData.ClienteCodice = wingsBolleEmesse.ClienteCodice;
                    dbBEData.ClRagSoc = wingsBolleEmesse.ClRagSoc;
                    dbBEData.Bolla = wingsBolleEmesse.Bolla;
                    dbBEData.Slip = wingsBolleEmesse.Slip;
                    dbBEData.DocumentoTipo = wingsBolleEmesse.DocumentoTipo;
                    dbBEData.CentroDiCosto = wingsBolleEmesse.CentroDiCosto;
                    dbBEData.ImportazioneNumero = dbBEData.ImportazioneNumero + 1;
                    dbBEData.AnnoFattura = wingsBolleEmesse.AnnoFattura;
                    dbBEData.NomeAllegati = wingsBolleEmesse.NomeAllegati;
                    dbBEData.NomeFile = wingsBolleEmesse.NomeFile;
                    dbBEData.ImportazioneDataOra = DateTime.Now;
                }
                else
                {
                    //Create
                    wingsBolleEmesse.ImportazioneNumero = 1;

                    entity.WingsBolleEmesses.Add(wingsBolleEmesse);
                }

                int countSqlResponseItem = 0;
                countSqlResponseItem = await entity.SaveChangesAsync();

                retBE = await entity.WingsBolleEmesses.Where(x => x.Slip == wingsBolleEmesse.Slip
                                                                && x.AgenziaCWTCodice == wingsBolleEmesse.AgenziaCWTCodice
                                                                && x.PraticaNumeroWings == wingsBolleEmesse.PraticaNumeroWings
                                                                && x.ClienteCodice == wingsBolleEmesse.ClienteCodice)
                                                        .SingleOrDefaultAsync();
            }

            return retBE;
        }
    }
}
