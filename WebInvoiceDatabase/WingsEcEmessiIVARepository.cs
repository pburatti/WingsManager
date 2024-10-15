using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoiceDatabase.Context;
using WebInvoiceDatabase.Models;

namespace WebInvoiceDatabase
{
    public interface IWingsEcEmessiIVARepository
    {
        Task<List<WingsEcEmessiIva>> InsertWingsEcEmessiIVAAsync(List<WingsEcEmessiIva> inputList);
        Task<bool> DeleteWingsEcEmessiIVAAsync(string ecNumero);
    }

    public class WingsEcEmessiIVARepository : BaseRepository, IWingsEcEmessiIVARepository
    {
        public WingsEcEmessiIVARepository(string connectionString) : base(connectionString) { }
        public async Task<List<WingsEcEmessiIva>> InsertWingsEcEmessiIVAAsync(List<WingsEcEmessiIva> inputList)
        {
            if (inputList == null) return null;

            int result = 0;
            List<WingsEcEmessiIva> recordUpdated = null;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                string ecNumero = null;
                foreach (var data in inputList)
                {
                    ecNumero = data.EcNumero;
                    entity.WingsEcEmessiIvas.Add(data);
                }
                
                result = await entity.SaveChangesAsync();
                recordUpdated = await entity.WingsEcEmessiIvas.Where(x => x.EcNumero == ecNumero).ToListAsync();
            }

            return recordUpdated;
        }
        public async Task<bool> DeleteWingsEcEmessiIVAAsync(string ecNumero)
        {
            int result = 0;
            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IEnumerable<WingsEcEmessiIva> datas = entity.WingsEcEmessiIvas.Where(x => x.EcNumero == ecNumero);
                if (datas != null)
                {
                    foreach (var data in datas)
                    {
                        entity.WingsEcEmessiIvas.Remove(data);
                    }
                    
                    result = await entity.SaveChangesAsync();
                }
            }

            return (result > 0);
        }
    }
}
