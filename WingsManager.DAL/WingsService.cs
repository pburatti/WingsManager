using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebInvoiceDatabase;
using WebInvoiceDatabase.Models;
using WingsManager.Model.Configurations;
using WingsManager.Model.Managers.Entity;

namespace WingsManager.DAL
{
    public interface IWingsService
    {
        Task<List<Product>> GetProductsAsync(string prodottoSigla, string prodottoDesc, CancellationToken cancellationToken);
    }

    public class WingsService : ServiceBase, IWingsService
    {
        public WingsService(IOptions<AppConfiguration> appConfig) : base (appConfig)
        {
        }
        public async Task<List<Product>> GetProductsAsync(string prodottoSigla, string prodottoDesc, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(prodottoDesc) && string.IsNullOrEmpty(prodottoSigla))
                return null;

            WingsProdottiRepository repository = new WingsProdottiRepository(base.ConnectionString);
            List<WingsProdotti> dbWingsProdotti = await repository.GetWingsProdottiAsync(prodottoSigla, prodottoDesc);
            return dbWingsProdotti.Select(x => new Product()
            {
                Id = x.IDProdotto,
                Code = x.ProdottoSigla,
                Description = x.ProdottoDesc,
                TypeId = x.IDTipoProdotto,
                TypeCode = x.TipoProdottoSigla,
                CCCode = x.CCSigla
            }).ToList();
        }
    }
}
