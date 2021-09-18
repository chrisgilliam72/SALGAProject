using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdateAsseessmentBlobMetaData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceprovider = new ServiceCollection()
                                        .AddDbContextPool<SALGADbContext>(options => options.UseSqlServer("server=salgahrpulsesqlsvr.database.windows.net;Database=SALGAHRPulse;User Id=HRPulseAdmin;Password=Password01+;"))
                                        .AddSingleton<IDemographicsRepository, SQLDemographicsRepository>()
                                        .BuildServiceProvider();

            var dbContext = serviceprovider.GetService<IDemographicsRepository>();
            var municipalities = await dbContext.GetMunicipalities();

            var containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=hrpulsestorage;AccountKey=v3ZLt/tCpHvQUVT5y/jTBySahzDM/zedaCWW8yKSmCFMZXSlietcbuEd/C5yLMmVMjHyPZsdpC6mJtd8plzntA==;EndpointSuffix=core.windows.net", "pastassessments");
            var blobInfos = containerClient.GetBlobs(Azure.Storage.Blobs.Models.BlobTraits.Metadata).ToList();

            foreach (var municipality in municipalities)
            {
                var municpalityName = municipality.Name.TrimEnd(' ');
                var municipalityBlobInfo = blobInfos.FirstOrDefault(x => x.Name.Contains(municpalityName));
                if (municipalityBlobInfo!=null)
                {
                    var blobClient = containerClient.GetBlobClient(municipalityBlobInfo.Name);

                    IDictionary<string, string> metadata = new Dictionary<string, string>();

                    // Add metadata to the dictionary by calling the Add method
                    metadata.Add("Municipality", municpalityName);
                    metadata.Add("Year", "2020");
                    // Add metadata to the dictionary by using key/value syntax

                    try
                    {
                        // Set the blob's metadata.
                        await blobClient.SetMetadataAsync(metadata);
                        Console.WriteLine("Updated " + municpalityName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("unable to update:  " + municpalityName + " :" + ex.Message);
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
