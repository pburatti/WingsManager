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
    public interface IClientiRepository
    {
        Task<List<Clienti>> GetClienteAsync(decimal? clienteCodice, CancellationToken cancellationToken);
    }

    public class ClientiRepository: BaseRepository, IClientiRepository
    {
        public ClientiRepository(string connectionString) : base(connectionString) { }
        public async Task<List<Clienti>> GetClienteAsync(decimal? clienteCodice, CancellationToken cancellationToken)
        {
            List<Clienti> dbData = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<Clienti> query = entity.Clientis;

                if (clienteCodice.HasValue)
                    query = query.Where(x => x.ClienteCodice == clienteCodice.Value);

                dbData = await query.ToListAsync();
            }

            return dbData;
        }
    }
}
