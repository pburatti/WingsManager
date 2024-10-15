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

namespace WingsManager.DAL
{
    public interface IRepositoryService
    {
        Task<Client> GetClientByCodeAsync(int clientCode, CancellationToken cancellationToken);
        Task<Supplier> GetSupplierByHarpCodeAsync(decimal harpCode, CancellationToken cancellationToken);
        Task<Agency> GetCwtAgencyAsync(CancellationToken cancellationToken, int? agencyCode = null, int? countryCwtCode = null);
        Task<EmailFaxSendData> GetInvioFaxEmailAsync(CancellationToken cancellationToken, int? idInvioFaxEmail = null);
        Task<int> InsertInvioFaxEmailAsync(EmailFaxSendData inputData, CancellationToken cancellationToken);
        Task<int> MassiveUpdateVoucherStateWithoutSupplierAsync(CancellationToken cancellationToken);
        Task<int> MassiveUpdateVoucherOvernightStayPreviousAsync(CancellationToken cancellationToken);
    }
    public class RepositoryService: ServiceBase, IRepositoryService
    {
        private readonly ILogger<RepositoryService>  _logger = null;

        public RepositoryService(ILogger<RepositoryService> logger, IOptions<AppConfiguration> appConfig) : base(appConfig)
        {
            this._logger = logger;
        }

        public async Task<Client> GetClientByCodeAsync(int clientCode, CancellationToken cancellationToken)
        {
            if (clientCode <= 0)
                return null;

            IClientiRepository repository = new ClientiRepository(base.ConnectionString);
            var data = await repository.GetClienteAsync(clientCode, cancellationToken);
            if (data == null)
            { 
            }

            return data.Select(x => new Client()
            {
                Id = int.Parse(x.AziendaCodice.GetValueOrDefault(0).ToString()),
                Code = int.Parse(x.AziendaCodice.GetValueOrDefault(0).ToString()),
                Name = x.RagSoc,
                CwtAgencyCode = Convert.ToInt32(x.AgenziaCwtCodice),
                WingsDeliveryCode = x.WingsCodConsCont
            }).LastOrDefault();
        }
        public async Task<Supplier> GetSupplierByHarpCodeAsync(decimal harpCode, CancellationToken cancellationToken)
        {
            if (harpCode <= 0)
                return null;

            IFornitoriRepository repository = new FornitoriRepository(base.ConnectionString);
            var data = await repository.GetFornitoriAsync(harpCode, cancellationToken);
            if (data == null)
            {
            }

            return data.Select(x => new Supplier()
            {
                Id = x.IDFornitore,
                Code = Convert.ToInt32(x.FornitoreCodice),
                Name = x.RagSoc,
                FaxNumber = string.IsNullOrEmpty(x.FaxNumCwt)?x.Fax:x.FaxNumCwt,
                CountryCode = x.NazioneSigla,
                EmailAddress = x.EmailInvioVoucher,
                SendVoucherType = Convert.ToInt32(x.TipoInvioVoucher)
            }).LastOrDefault();
        }
        public async Task<Agency> GetCwtAgencyAsync(CancellationToken cancellationToken, int? agencyCode = null, int? countryCwtCode = null)
        {
            IAgenzieCwtRepository repository = new AgenzieCwtRepository(base.ConnectionString);
            var data = await repository.GetAgenzieCwtAsync(cancellationToken, agencyCode, countryCwtCode);
            if (data == null)
            {
            }

            return data.Select(x => new Agency()
            {
                VoucherTypeSending = x.InvioVoucherTipo,
                VoucherSr56Sending = x.InvioVcrSr56
            }).LastOrDefault();
        }
        public async Task<EmailFaxSendData> GetInvioFaxEmailAsync(CancellationToken cancellationToken, int? idInvioFaxEmail = null)
        {
            IInvioFaxEmailRepository repository = new InvioFaxEmailRepository(base.ConnectionString);
            var data = await repository.GetInvioFaxEmailAsync(cancellationToken, idInvioFaxEmail);
            if (data == null)
            {
            }

            return data.Select(x => new EmailFaxSendData()
            {
                Id = x.IdInvioFaxEmail,
                ApplicationId = x.IdApplicazione,
                DocumentId = x.IdDocumento,
                EmailFax = x.FaxEmail,
                EmailFaxDestination = x.FaxEmailDest,
                ReceiverType = x.IdTipoDestinatario,
                SenderEmailAddress = x.EmailMittente,
                SenderName = x.NomeMittente,
                SentDate = x.DataOraInvio,
                State = x.Stato,
                StateDescription = x.DescStato
            }).LastOrDefault();
        }
        public async Task<int> InsertInvioFaxEmailAsync(EmailFaxSendData inputData, CancellationToken cancellationToken)
        {
            if (inputData == null)
                return 0;

            IInvioFaxEmailRepository repository = new InvioFaxEmailRepository(base.ConnectionString);

            InvioFaxEmail dbModel = new InvioFaxEmail();
            dbModel.IdApplicazione = inputData.ApplicationId;
            dbModel.IdDocumento = inputData.DocumentId;
            dbModel.NomeMittente = inputData.SenderName;
            dbModel.EmailMittente = inputData.SenderEmailAddress;
            dbModel.IdTipoDestinatario = inputData.ReceiverType;
            dbModel.FaxEmail = inputData.EmailFax;
            dbModel.FaxEmailDest = inputData.EmailFaxDestination;
            dbModel.Stato = inputData.State;
            dbModel.DescStato = inputData.StateDescription;
            dbModel.DataOraInvio = inputData.SentDate;

            int result = await repository.InsertInvioFaxEmailAsync(dbModel, cancellationToken);

            return result;
        }
        public async Task<int> MassiveUpdateVoucherStateWithoutSupplierAsync(CancellationToken cancellationToken)
        {
            IBulkMassiveRepository repository = new BulkMassiveRepository(base.ConnectionString);
            return await repository.MassiveUpdateVoucherStateWithoutSupplierAsync(cancellationToken);
        }
        public async Task<int> MassiveUpdateVoucherOvernightStayPreviousAsync(CancellationToken cancellationToken)
        {
            IBulkMassiveRepository repository = new BulkMassiveRepository(base.ConnectionString);
            return await repository.MassiveUpdateVoucherOvernightStayPreviousAsync(cancellationToken);
        }
    }
}
