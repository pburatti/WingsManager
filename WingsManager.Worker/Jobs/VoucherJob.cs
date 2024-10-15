using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WingsManager.BLL;

namespace WingsManager.Worker.Jobs
{
    [DisallowConcurrentExecution]
    public class VoucherJob : IJob
    {
        private readonly ILogger<VoucherJob> _logger = null;
        private readonly IVoucherManager _manager = null;

        public VoucherJob(ILogger<VoucherJob> logger, IVoucherManager manager)
        {
            this._logger = logger;
            this._manager = manager;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await this._manager.ImportVoucherWorker(context.CancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, "A critical error has occurred that does not allow the correct execution of the job");
                //throw ex;
            }
        }
    }
}
