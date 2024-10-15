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
    public interface IInvioFaxEmailRepository
    {
        Task<List<InvioFaxEmail>> GetInvioFaxEmailAsync(CancellationToken cancellationToken, int? idInvioFaxEmail = null);
        Task<int> InsertInvioFaxEmailAsync(InvioFaxEmail inputData, CancellationToken cancellationToken);
        Task<int> UpdateInvioFaxEmailAsync(InvioFaxEmail inputData, CancellationToken cancellationToken);
    }

    public class InvioFaxEmailRepository : BaseRepository, IInvioFaxEmailRepository
    {
        public InvioFaxEmailRepository(string connectionString) : base(connectionString) { }
        public async Task<List<InvioFaxEmail>> GetInvioFaxEmailAsync(CancellationToken cancellationToken, int? idInvioFaxEmail = null)
        {
            List<InvioFaxEmail> dbData = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<InvioFaxEmail> query = entity.InvioFaxEmails;

                if (idInvioFaxEmail.HasValue)
                    query = query.Where(x => x.IdInvioFaxEmail == idInvioFaxEmail.Value);

                dbData = await query.ToListAsync(cancellationToken);
            }

            return dbData;
        }

        public async Task<int> InsertInvioFaxEmailAsync(InvioFaxEmail inputData, CancellationToken cancellationToken)
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
                        
                        long maxId = (entity.InvioFaxEmails.Any()? entity.InvioFaxEmails.Max(m => m.IdInvioFaxEmail) : 0)+1;
                        //long maxId = await entity.InvioFaxEmails.OrderByDescending(x => x.IdInvioFaxEmail).MaxAsync(x=> x.IdInvioFaxEmail, cancellationToken);
                        inputData.IdInvioFaxEmail = maxId;

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
        public async Task<int> UpdateInvioFaxEmailAsync(InvioFaxEmail inputData, CancellationToken cancellationToken)
        {
            if (inputData == null)
                return 0;

            int result = 0;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                //InvioFaxEmail dbData = (await this.GetInvioFaxEmailAsync(cancellationToken, inputData.IdInvioFaxEmail)).LastOrDefault();
                //if (dbData!= null)
                //{
                //    entity.Attach(inputData);
                //}
                entity.Attach(inputData);
                result = await entity.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
        

    }
}
