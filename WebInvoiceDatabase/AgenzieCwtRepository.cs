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
    public interface IAgenzieCwtRepository
    {
        Task<List<AgenzieCwt>> GetAgenzieCwtAsync(CancellationToken cancellationToken, int? agenziaCWTCodice = null, int? paeseCwtCodice = null);
    }

    public class AgenzieCwtRepository : BaseRepository, IAgenzieCwtRepository
    {
        public AgenzieCwtRepository(string connectionString) : base(connectionString) { }
        
        public async Task<List<AgenzieCwt>> GetAgenzieCwtAsync(CancellationToken cancellationToken, int? agenziaCWTCodice = null, int? paeseCwtCodice = null)
        {
            List<AgenzieCwt> dbData = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<AgenzieCwt> query = entity.AgenzieCwts;

                if (agenziaCWTCodice.HasValue)
                    query = query.Where(x => x.AgenziaCwtCodice == agenziaCWTCodice.Value);
                if (paeseCwtCodice.HasValue)
                    query = query.Where(x => x.PaeseCwtCodice == paeseCwtCodice.Value);

                dbData = await query.ToListAsync(cancellationToken);
            }

            return dbData;
        }
    }
}
