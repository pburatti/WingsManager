
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System;
using System.IO;
using WingsManager.BLL;
using WingsManager.BLL.MailSender;
using WingsManager.DAL;
using WingsManager.ExternalService.Genesis;
using WingsManager.Model.Configurations;

namespace WingsManager.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Serilog application logging
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var appConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(appConfiguration)
                //.WriteTo.e
                .CreateLogger();
            #endregion

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    #region Quartz scheduler base config
                    var builder = new ConfigurationBuilder().AddJsonFile("quartz.json");
                    IConfiguration quartzConfiguration = builder.Build();
                    services.Configure<QuartzOptions>(quartzConfiguration.GetSection("Quartz"));
                    services.AddQuartz(q =>
                    {
                        q.SchedulerId = "Scheduler-Core";
                        q.UseMicrosoftDependencyInjectionJobFactory();
                    });

                    services.AddQuartzHostedService(options =>
                    {
                        options.WaitForJobsToComplete = true;
                    });
                    #endregion

                    #region Configuration
                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                    var appConfiguration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", true)
                        .Build();

                    services.Configure<AppConfiguration>(appConfiguration.GetSection("AppSettings"));
                    #endregion

                    //DI Managers
                    services
                        .AddScoped<IAccountStatementManager, AccountStatementManager>()
                        .AddScoped<IBillManager, BillManager>()
                        .AddScoped<IVoucherManager, VoucherManager>()
                        .AddScoped<IVoucherMSFManager, VoucherMSFManager>()
                        .AddScoped<ISendEmail, SendEmail>()
                    //DI Services
                        .AddScoped<IAccountStatementService, AccountStatementService>()
                        .AddScoped<IWingsService, WingsService>()
                        .AddScoped<IBillService, BillService>()
                        .AddScoped<IGenesisService, GenesisService>()
                        .AddScoped<IVoucherService, VoucherService>()
                        .AddScoped<IRepositoryService, RepositoryService>();

                    services.AddHttpClient<IGenesisCoreApi,GenesisCoreApi>(c =>
                    {
                        c.BaseAddress = new Uri(appConfiguration.GetSection("ExternalServicesConfig:GenesisCoreApiConfig:Address").Value);
                        c.DefaultRequestHeaders.TryAddWithoutValidation("usr", appConfiguration.GetSection("ExternalServicesConfig:GenesisCoreApiConfig:User").Value);
                        var timeOut = appConfiguration.GetSection("ExternalServicesConfig:GenesisCoreApiConfig:Timeout");
                        if (timeOut != null)
                            c.Timeout = TimeSpan.FromSeconds(double.Parse(timeOut.Value));
                        else
                            c.Timeout = TimeSpan.FromSeconds(20);
                    });
                });
    }
}
