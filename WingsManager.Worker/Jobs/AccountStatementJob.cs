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
    public class AccountStatementJob : IJob
    {
        private readonly ILogger<AccountStatementJob> _logger = null;
        private readonly IAccountStatementManager _manager = null;

        public AccountStatementJob(ILogger<AccountStatementJob> logger, IAccountStatementManager manager)
        {
            this._logger = logger;
            this._manager = manager;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await this._manager.ImportWingsAccountStatementWorker(context.CancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, "A critical error has occurred that does not allow the correct execution of the job");
                //throw ex;
            }
        }
    }
}
