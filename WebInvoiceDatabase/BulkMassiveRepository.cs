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
    public interface IBulkMassiveRepository
    {
        Task<int> MassiveUpdateVoucherStateWithoutSupplierAsync(CancellationToken cancellationToken);
        Task<int> MassiveUpdateVoucherOvernightStayPreviousAsync(CancellationToken cancellationToken);
    }

    public class BulkMassiveRepository : BaseRepository, IBulkMassiveRepository
    {
        public BulkMassiveRepository(string connectionString) : base(connectionString) { }
        public async Task<int> MassiveUpdateVoucherStateWithoutSupplierAsync(CancellationToken cancellationToken)
        {
            int dbResult = 0;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                //dbresult = 
                dbResult = await entity.Database.ExecuteSqlRawAsync("EXEC efMassiveUpdateVoucherWithoutSupplierCopy", cancellationToken);
            }

            return dbResult;
        }
        public async Task<int> MassiveUpdateVoucherOvernightStayPreviousAsync(CancellationToken cancellationToken)
        {
            int dbResult = 0;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                dbResult = await entity.Database.ExecuteSqlRawAsync("EXEC efMassiveUpdateVoucherOvernightStayPrevious", cancellationToken);
            }

            return dbResult;
        }
    }
}
