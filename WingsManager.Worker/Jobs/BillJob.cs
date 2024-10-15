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
    public class BillJob : IJob
    {
        private readonly ILogger<BillJob> _logger = null;
        private readonly IBillManager _manager = null;

        public BillJob(ILogger<BillJob> logger, IBillManager manager)
        {
            this._logger = logger;
            this._manager = manager;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await this._manager.ImportWingsBillWorker(context.CancellationToken);
            }
            catch (Exception ex)
            {
                //toDo:send mail
                this._logger.LogCritical(ex, "A critical error has occurred that does not allow the correct execution of the job");
                //throw ex;
            }
        }
    }
}
