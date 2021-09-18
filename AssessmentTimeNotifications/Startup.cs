using AzureFunctionsSharedModelLib;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using SALGAEvidenceRepository;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(AssessmentTimeNotifications.Startup))]
namespace AssessmentTimeNotifications
{
    class Startup : FunctionsStartup
    {


        public Startup()
        {
           
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //string cs = Environment.GetEnvironmentVariable("ConnectionString");
            //builder.ConfigurationBuilder.AddAzureAppConfiguration(cs);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            var dbConnectonString = config.GetConnectionString("DBConnectionString");

            //builder.Services.AddSingleton<IEvdenceRepository, AzureStorageRepository>();
            builder.Services.AddDbContext<SALGADBContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, dbConnectonString));
        }
    }
}
