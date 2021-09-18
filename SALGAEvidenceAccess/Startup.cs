using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using SALGAEvidenceRepository;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(SALGAEvidenceAccess.Startup))]
namespace SALGAEvidenceAccess
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
            //builder.Services.AddSingleton<IEvdenceRepository, AzureStorageRepository>();
            builder.Services.AddDbContext<SALGADBContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options,"server = salgasadb.database.windows.net; Database = SALGA; User Id = chrisg; Password = Password01+;"));
        }
    }
}
