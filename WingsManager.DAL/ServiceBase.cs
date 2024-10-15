using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using WebInvoiceDatabase.Context;
using WingsManager.Model.Configurations;

namespace WingsManager.DAL
{
    public abstract class ServiceBase
    {
        private readonly IOptions<AppConfiguration> _appConfig = null;
        //private readonly DbContextOptionsBuilder _ctxBuilder = null;
        private readonly string _connectionString = null;
        private readonly EmailConfig _emailCfg = null;
         

        //public DbContextOptionsBuilder ContextBuilder { get { return this._ctxBuilder; } }
        public string ConnectionString { get { return this._connectionString; } }
        public EmailConfig emailConfig {get {return this._emailCfg;} }

        public ServiceBase(IOptions<AppConfiguration> appConfig)
        {
            this._appConfig = appConfig;
            this._connectionString = this._appConfig.Value.DatabaseConnections.WebInvoice;
            this._emailCfg = this._appConfig.Value.EmailConfig;
            //this._ctxBuilder = new DbContextOptionsBuilder();
            //this._ctxBuilder.UseSqlServer(this._connectionString);
        }
    }
}
