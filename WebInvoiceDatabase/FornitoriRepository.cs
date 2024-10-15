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
    public interface IFornitoriRepository
    {
        Task<List<Fornitori>> GetFornitoriAsync(decimal? harpCodice, CancellationToken cancellationToken);
    }

    public class FornitoriRepository : BaseRepository, IFornitoriRepository
    {
        public FornitoriRepository(string connectionString) : base(connectionString) { }
        public async Task<List<Fornitori>> GetFornitoriAsync(decimal? harpCodice, CancellationToken cancellationToken)
        {
            List<Fornitori> dbData = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<Fornitori> query = entity.Fornitoris;

                if (harpCodice.HasValue)
                    query = query.Where(x => x.HarpCodice == harpCodice);

                dbData = await query.ToListAsync();
            }

            return dbData;
        }
    }
}
