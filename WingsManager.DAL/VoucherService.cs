using Microsoft.Extensions.Logging;
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
using WingsManager.Model.Managers.Voucher;

namespace WingsManager.DAL
{
    public interface IVoucherService
    {
        Task<int> InsertOrUpdateVoucherAsync(ImportDataWings importData, CancellationToken cancellationToken);
        Task<int> SetInvioFaxEmailInviatiAsync(long idVoucherEmesso, short inviato, int lastSend, CancellationToken cancellationToken);
    }
    public class VoucherService : ServiceBase, IVoucherService
    {
        private readonly ILogger<VoucherService> _logger = null;
        public VoucherService(ILogger<VoucherService> logger, IOptions<AppConfiguration> appConfig) : base(appConfig)
        {
            this._logger = logger;
        }
        public async Task<List<Voucher>> GetVouchersAsync(long? id, CancellationToken cancellationToken)
        {
            IWingsVoucherEmessiRepository repository = new WingsVoucherEmessiRepository(base.ConnectionString);
            IEnumerable<WingsVoucherEmessi> datas = await repository.GetWingsVoucherEmessiAsync(cancellationToken, id);
            return datas.Select(x => new Voucher()
            {
                ArchiveFileName = x.NomeFileAcquisito,
                //todo: problem
                BillingDocument = x.VoucherCliente,
                ClientCode = x.ClienteCodice,
                ClientName = x.ClienteDescrizione,
                CwtAgencyCode = (x.AgenziaCwtCodice.HasValue) ? Convert.ToInt32(x.HarpCodice) : (int?)null,
                DataIn = x.DataIn,
                DataOut = x.DataOut,
                DocumentId = x.PraticaNumeroWings,
                DocumentIssueDate = x.EmissioneData,
                HarpCode = (!string.IsNullOrEmpty(x.HarpCodice)) ? Convert.ToInt32(x.HarpCodice) : (int?)null,
                Id = x.IdVoucherEmesso,
                //isVoucherCliente = 
                PassengerName = x.PaxNome,
                //SupplierCountryCode = x.forn
                SupplierDeliveryCode = x.WingsCodConsCont,
                SupplierDescription = x.FornitoreDescrizione,
                //SupplierEmailAddress = x.for
                SupplierFax = x.FornitoreFax,
                TotalAmount = x.ImportoTotaleCliente,
                VoucherNumber = x.VoucherNumero,
                VoucherType = x.VoucherType,
                VoucherSeries = x.VoucherSerie,
                WingsProductCode = x.ProdottoSigla
            }).ToList();
        }
        public async Task<int> InsertOrUpdateVoucherAsync(ImportDataWings importData, CancellationToken cancellationToken)
        {
            if (importData == null)
                return 0;

            int result = 0;
            IWingsVoucherEmessiRepository repository = new WingsVoucherEmessiRepository(base.ConnectionString);
            //IWingsProdottiRepository wingsProductRepository = new WingsProdottiRepository();

            WingsVoucherEmessi model = new WingsVoucherEmessi();
            model.AgenziaCwtCodice = importData.CwtAgencyCode;
            model.PraticaNumeroWings = importData.DocumentId;
            model.VoucherSerie = importData.VoucherSeries;
            model.VoucherNumero = importData.VoucherNumber;
            model.ClienteCodice = importData.ClientCode;
            model.ClienteDescrizione = importData.ClientName;
            model.PaxNome = importData.PassengerName;
            model.FornitoreFax = importData.SupplierFax;
            model.FornitoreDescrizione = importData.SupplierDescription;
            model.VoucherCliente = (importData.isVoucherCliente) ? importData.BillingDocument : null;
            model.VoucherFornitore = (!importData.isVoucherCliente) ? importData.BillingDocument : null;
            model.NomeFileAcquisito = importData.ArchiveFileName;
            model.EmissioneData = importData.DocumentIssueDate;
            model.DataIn = importData.DataIn;
            model.DataOut = importData.DataOut;
            model.VoucherType = importData.VoucherType;
            model.HarpCodice = importData.HarpCode?.ToString();
            model.WingsCodConsCont = importData.SupplierDeliveryCode;
            model.NazioneSigla = importData.SupplierCountryCode;
            model.FlgPagatoCC = importData.FlgPaidCC;
            model.ProdottoSigla = importData.WingsProductCode;
            model.ImportoTotaleCliente = (importData.isVoucherCliente) ? importData.TotalAmount: null;
            model.ImportoTotale = (!importData.isVoucherCliente) ? importData.TotalAmount : null;
            WingsVoucherEmessi voucher = (await repository.GetWingsVoucherEmessiAsync(cancellationToken, null, importData.CwtAgencyCode, importData.VoucherSeries, importData.VoucherNumber)).LastOrDefault();
            if (voucher != null)
            {
                voucher.AgenziaCwtCodice = model.AgenziaCwtCodice;
                voucher.PraticaNumeroWings = model.PraticaNumeroWings;
                voucher.VoucherSerie = model.VoucherSerie;
                voucher.VoucherNumero = model.VoucherNumero;
                voucher.ClienteCodice = model.ClienteCodice;
                voucher.ClienteDescrizione = model.ClienteDescrizione;
                voucher.PaxNome = model.PaxNome;
                voucher.FornitoreFax = model.FornitoreFax;
                voucher.FornitoreDescrizione = model.FornitoreDescrizione;
                voucher.VoucherCliente =  model.VoucherCliente?? voucher.VoucherCliente;
                voucher.VoucherFornitore = model.VoucherFornitore?? voucher.VoucherFornitore;
                voucher.NomeFileAcquisito = model.NomeFileAcquisito;
                voucher.EmissioneData = model.EmissioneData;
                voucher.DataIn = model.DataIn;
                voucher.DataOut = model.DataOut;
                voucher.VoucherType = model.VoucherType;
                voucher.HarpCodice = model.HarpCodice;
                voucher.WingsCodConsCont = model.WingsCodConsCont;
                voucher.NazioneSigla = model.NazioneSigla;
                voucher.FlgPagatoCC = model.FlgPagatoCC;
                voucher.ProdottoSigla = model.ProdottoSigla;
                voucher.ImportoTotaleCliente = (importData.isVoucherCliente) ? importData.TotalAmount: voucher.ImportoTotaleCliente;
                voucher.ImportoTotale = (!importData.isVoucherCliente) ? importData.TotalAmount : voucher.ImportoTotale;

                voucher.AcquisizioneDataOra = DateTime.Now;

                result = await repository.UpdateWingsVoucherEmessiAsync(voucher, cancellationToken);
                importData.VoucherIdInserted = voucher.IdVoucherEmesso;
            }
            else
            {
                result = await repository.InsertWingsVoucherEmessiAsync(model, cancellationToken);
                importData.VoucherIdInserted = model.IdVoucherEmesso;
            }

            return result;
        }
        public async Task<int> InsertVoucherAsync(Voucher importData, CancellationToken cancellationToken)
        {
            if (importData == null)
                return 0;

            int result = 0;
            IWingsVoucherEmessiRepository repository = new WingsVoucherEmessiRepository(base.ConnectionString);
            WingsVoucherEmessi model = new WingsVoucherEmessi();

            model.AgenziaCwtCodice = importData.CwtAgencyCode;
            model.PraticaNumeroWings = importData.DocumentId;
            model.VoucherSerie = importData.VoucherSeries;
            model.VoucherNumero = importData.VoucherNumber;
            model.ClienteCodice = importData.ClientCode;
            model.ClienteDescrizione = importData.ClientName;
            model.PaxNome = importData.PassengerName;
            model.FornitoreFax = importData.SupplierFax;
            model.FornitoreDescrizione = importData.SupplierDescription;
            model.VoucherCliente = (importData.isVoucherCliente.HasValue) ? importData.BillingDocument : null;
            model.VoucherFornitore = (!importData.isVoucherCliente.HasValue) ? importData.BillingDocument : null;
            model.NomeFileAcquisito = importData.ArchiveFileName;
            model.EmissioneData = importData.DocumentIssueDate;
            model.DataIn = importData.DataIn;
            model.DataOut = importData.DataOut;
            model.VoucherType = importData.VoucherType;
            model.HarpCodice = importData.HarpCode.ToString();
            model.WingsCodConsCont = importData.SupplierDeliveryCode;
            model.NazioneSigla = importData.SupplierCountryCode;
            model.FlgPagatoCC = importData.FlgPaidCC;
            model.ProdottoSigla = importData.WingsProductCode;
            model.ImportoTotaleCliente = importData.TotalAmount;

            result = await repository.InsertWingsVoucherEmessiAsync(model, cancellationToken);
            importData.Id = model.IdVoucherEmesso;

            return result;
        }
        public async Task<int> UpdateVoucherAsync(Voucher importData, CancellationToken cancellationToken)
        {
            if (importData == null)
                return 0;

            int result = 0;
            IWingsVoucherEmessiRepository repository = new WingsVoucherEmessiRepository(base.ConnectionString);
            WingsVoucherEmessi voucher = (await repository.GetWingsVoucherEmessiAsync(cancellationToken, null, importData.CwtAgencyCode, importData.VoucherSeries, importData.VoucherNumber)).LastOrDefault();

            if (voucher != null)
            {
                voucher.AgenziaCwtCodice = importData.CwtAgencyCode;
                voucher.PraticaNumeroWings = importData.DocumentId;
                voucher.VoucherSerie = importData.VoucherSeries;
                voucher.VoucherNumero = importData.VoucherNumber;
                voucher.ClienteCodice = importData.ClientCode;
                voucher.ClienteDescrizione = importData.ClientName;
                voucher.PaxNome = importData.PassengerName;
                voucher.FornitoreFax = importData.SupplierFax;
                voucher.FornitoreDescrizione = importData.SupplierDescription;
                voucher.VoucherCliente = (importData.isVoucherCliente.HasValue) ? importData.BillingDocument : null;
                voucher.VoucherFornitore = (!importData.isVoucherCliente.HasValue) ? importData.BillingDocument : null;
                voucher.NomeFileAcquisito = importData.ArchiveFileName;
                voucher.EmissioneData = importData.DocumentIssueDate;
                voucher.DataIn = importData.DataIn;
                voucher.DataOut = importData.DataOut;
                voucher.VoucherType = importData.VoucherType;
                voucher.HarpCodice = importData.HarpCode.ToString();
                voucher.WingsCodConsCont = importData.SupplierDeliveryCode;
                voucher.NazioneSigla = importData.SupplierCountryCode;
                voucher.FlgPagatoCC = importData.FlgPaidCC;
                voucher.ProdottoSigla = importData.WingsProductCode;
                voucher.ImportoTotaleCliente = (importData.isVoucherCliente.HasValue) ? importData.TotalAmount : voucher.ImportoTotaleCliente;
                voucher.ImportoTotale = (!importData.isVoucherCliente.HasValue) ? importData.TotalAmount : voucher.ImportoTotale;

                result = await repository.UpdateWingsVoucherEmessiAsync(voucher, cancellationToken);
                importData.Id = voucher.IdVoucherEmesso;
            }

            return result;
        }

        public async Task<int> SetInvioFaxEmailInviatiAsync(long idVoucherEmesso, short inviato, int lastSend, CancellationToken cancellationToken)
        {
            //int result = 0;
            IWingsVoucherEmessiRepository repository = new WingsVoucherEmessiRepository(base.ConnectionString);
            return await repository.SetInvioFaxEmailInviatiAsync(idVoucherEmesso, inviato, lastSend, cancellationToken);
        }
    }
}
