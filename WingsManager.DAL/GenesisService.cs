using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WingsManager.ExternalService.Genesis;
using WingsManager.Model.Configurations;

namespace WingsManager.DAL
{
    public interface IGenesisService
    {
        Task<string> GetCustomerNameByWingsCodeAsync(string wingsId, CancellationToken cancellationToken);
    }

    public class GenesisService : ServiceBase, IGenesisService
    {
        private readonly ILogger<GenesisService> _logger = null;
        private readonly IGenesisCoreApi _genesisCoreService = null;
        public GenesisService(ILogger<GenesisService> logger, IOptions<AppConfiguration> appConfig, IGenesisCoreApi genesisCoreService) : base(appConfig)
        {
            this._logger = logger;
            this._genesisCoreService = genesisCoreService;
        }
        public async Task<string> GetCustomerNameByWingsCodeAsync(string wingsId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(wingsId))
                return null;

            string customerName = null;
            var response = await this._genesisCoreService.GetCustomerListAsync(new GenesisCoreInterface.Customer.RequestResponse.CompanyListGet_RQ() {  acccd  = wingsId }, cancellationToken);
            if (response.scs && response.cmps?.Length > 0)
            {
                customerName = response.cmps[0].nm;
            }
            return customerName;
        }
    }
}
