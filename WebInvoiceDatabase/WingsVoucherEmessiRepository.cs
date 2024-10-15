using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebInvoiceDatabase.Context;
using WebInvoiceDatabase.Models;

namespace WebInvoiceDatabase
{
    public interface IWingsVoucherEmessiRepository
    {
        Task<List<WingsVoucherEmessi>> GetWingsVoucherEmessiAsync(CancellationToken cancellationToken, long? id, int? cwtAgencyCode = null, string voucherSeries = null, string voucherNumero = null);
        Task<int> InsertWingsVoucherEmessiAsync(WingsVoucherEmessi inputData, CancellationToken cancellationToken);
        Task<int> UpdateWingsVoucherEmessiAsync(WingsVoucherEmessi inputData, CancellationToken cancellationToken);
        Task<int> SetInvioFaxEmailInviatiAsync(long idVoucherEmesso, short inviato, int lastSend, CancellationToken cancellationToken);
    }

    public class WingsVoucherEmessiRepository : BaseRepository, IWingsVoucherEmessiRepository
    {
        public WingsVoucherEmessiRepository(string connectionString) : base(connectionString) { }
        public async Task<List<WingsVoucherEmessi>> GetWingsVoucherEmessiAsync(CancellationToken cancellationToken, long? id, int? cwtAgencyCode = null, string voucherSeries = null, string voucherNumero = null)
        {
            List<WingsVoucherEmessi> dbData = null;

                using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
                {
                    IQueryable<WingsVoucherEmessi> query = entity.WingsVoucherEmessis;

                    if (id.HasValue)
                        query = query.Where(x => x.IdVoucherEmesso == id);
                    else
                        query = query.Where(w => w.AgenziaCwtCodice == cwtAgencyCode && w.VoucherSerie == voucherSeries && w.VoucherNumero == voucherNumero);//è considerata una chiave multipla
                    /*
                    if (cwtAgencyCode.HasValue)
                        query = query.Where(x => x.AgenziaCwtCodice == cwtAgencyCode.Value);
                    if (!string.IsNullOrEmpty(voucherSeries))
                        query = query.Where(x => x.VoucherSerie == voucherSeries);
                    if (!string.IsNullOrEmpty(voucherNumero))
                        query = query.Where(x => x.VoucherNumero == voucherNumero);
                    */
                    dbData = await query.ToListAsync(cancellationToken);
                }

            return dbData;
        }

        public async Task<int> InsertWingsVoucherEmessiAsync(WingsVoucherEmessi inputData, CancellationToken cancellationToken)
        {
            if (inputData == null)
                return 0;

            int result = 0;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                using (var transaction = await entity.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        WingsVoucherEmessi lastVoucher = await entity.WingsVoucherEmessis.OrderByDescending(x => x.IdVoucherEmesso).FirstOrDefaultAsync(cancellationToken);
                        inputData.IdVoucherEmesso = (lastVoucher?.IdVoucherEmesso??0) + 1;

                        await entity.AddAsync(inputData);
                        result = await entity.SaveChangesAsync();

                        await transaction.CommitAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw ex;
                    }
                }                
            }

            return result;
        }
        public async Task<int> UpdateWingsVoucherEmessiAsync(WingsVoucherEmessi inputData, CancellationToken cancellationToken)
        {
            if (inputData == null)
                return 0;

            int result = 0;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                WingsVoucherEmessi dbData = (await this.GetWingsVoucherEmessiAsync(cancellationToken, inputData.IdVoucherEmesso, Convert.ToInt32(inputData.AgenziaCwtCodice), inputData.VoucherSerie, inputData.VoucherNumero)).LastOrDefault();

                //WingsVoucherEmessi dbData = await entity.WingsVoucherEmessis.Where(w => w.IdVoucherEmesso == inputData.IdVoucherEmesso).LastOrDefaultAsync();
                //dbData = dbData == null ? await entity.WingsVoucherEmessis.Where(w => w.AgenziaCwtCodice == inputData.AgenziaCwtCodice && w.VoucherSerie == inputData.VoucherSerie && w.VoucherNumero == inputData.VoucherNumero).LastOrDefaultAsync() : dbData;//è considerata una chiave mulipla

                if (dbData != null)
                {
                    //entity.Attach(inputData);??
                    dbData.AcquisizioneDataOra = inputData.AcquisizioneDataOra;
                    entity.Update<WingsVoucherEmessi>(inputData);

                }
                result = await entity.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
        public async Task<int> SetInvioFaxEmailInviatiAsync(long idVoucherEmesso, short inviato, int lastSend, CancellationToken cancellationToken)
        {
            int result = 0;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                WingsVoucherEmessi wingsVoucherEmessi = await entity.WingsVoucherEmessis.Where(x => x.IdVoucherEmesso == idVoucherEmesso).SingleOrDefaultAsync(cancellationToken);

                if (wingsVoucherEmessi != null)
                {
                    wingsVoucherEmessi.Inviato = inviato;
                    wingsVoucherEmessi.LastSend = lastSend;

                    result = await entity.SaveChangesAsync(cancellationToken);
                }
            }

            return result;
        }
    }
}
