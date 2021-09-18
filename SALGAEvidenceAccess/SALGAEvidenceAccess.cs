using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SALGADBLib;
using Microsoft.EntityFrameworkCore;

using Azure.Storage.Blobs;

namespace SALGAEvidenceAccess
{

    //public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    //{
    //    string csv;

    //    //Do some stuff to create a csv

    //    byte[] filebytes = Encoding.UTF8.GetBytes(csv);

    //    return new FileContentResult(filebytes, "application/octet-stream")
    //    {
    //        FileDownloadName = "Export.csv"
    //    };
    //}

    public  class SALGAEvidenceAccess
    {
        private SALGADBContext _salgaDBContext;

        public SALGAEvidenceAccess(SALGADBContext salgaDBContext)
        {
            _salgaDBContext = salgaDBContext;
        }

        [FunctionName("SALGAEvidenceAccess")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "SALGAEvidenceAccess/{userID}/{evidenceID}")] HttpRequest req,
            String userID, int evidenceID)
        {

            try
            {
                var azureContainerName = Environment.GetEnvironmentVariable("AzureStorageContainerName");
                var azureConnectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString");
                Environment.GetEnvironmentVariable("MailHost");
                var evidence = await _salgaDBContext.AnswerEvidences.Include(x=>x.Municipality).FirstOrDefaultAsync(x => x.pkID == evidenceID);

                if (evidence!=null)
                {
                    var audititem = new AuditEvent()
                    {
                        ItemName = evidence.OriginalFileName,
                        EventType = "document access",
                        Date = DateTime.Now,
                        UserIDString=userID,
                        ItemPath=evidence.FileUrl,
                        UserEmail=userID,
                    };
                    _salgaDBContext.AuditEvents.Add(audititem); ;
                    _salgaDBContext.SaveChanges();

                    var containerClient = new BlobContainerClient(@"DefaultEndpointsProtocol=https;AccountName=salgaevidence;AccountKey=gz6GWyw/ynlD7sWVeJUFhzBXs2xXsHpCTR4m9COuUKl6hRKWR3C5l82WSxtY8C0Bvggl8uXfohoKqNVCbufY7g==;EndpointSuffix=core.windows.net",
                                                                    evidence.ContainerName);
                    var blobClient = containerClient.GetBlobClient(evidence.BlobName);

                    var memstream = new MemoryStream();
                    blobClient.DownloadTo(memstream);
                    var contentResult = new FileContentResult(memstream.ToArray(), "application/octet-stream");
                    contentResult.FileDownloadName = evidence.Municipality.Name+"-Municipality-"+evidence.OriginalFileName;
                    return contentResult;



                }



                return new ForbidResult();
            }
            catch (Exception ex)
            {
                return new  BadRequestObjectResult(ex.Message);

            }

           
        }
    }
}
