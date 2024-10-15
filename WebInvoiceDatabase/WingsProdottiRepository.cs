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
    public interface IWingsProdottiRepository
    {
        Task<List<WingsProdotti>> GetWingsProdottiAsync(string prodottoSigla, string prodottoDesc);
    }

    public class WingsProdottiRepository : BaseRepository, IWingsProdottiRepository
    {
        public WingsProdottiRepository(string connectionString) : base(connectionString) { }
        public async Task<List<WingsProdotti>> GetWingsProdottiAsync(string prodottoSigla = null, string prodottoDesc = null)
        {
            List<WingsProdotti> dbWingsProdotti = null;

            using (WebInvoiceContext entity = new WebInvoiceContext(base.ConnectionString))
            {
                IQueryable<WingsProdotti> queryWingsProdotti = entity.WingsProdottis;
                if (!string.IsNullOrEmpty(prodottoSigla))
                    queryWingsProdotti = queryWingsProdotti.Where(x => x.ProdottoSigla.ToUpper() == prodottoSigla.ToUpper());
                if (!string.IsNullOrEmpty(prodottoDesc))
                    queryWingsProdotti = queryWingsProdotti.Where(x => x.ProdottoDesc.ToUpper() == prodottoDesc.ToUpper());

                dbWingsProdotti = await queryWingsProdotti.ToListAsync();
            }

            return dbWingsProdotti;
        }
    }
}
