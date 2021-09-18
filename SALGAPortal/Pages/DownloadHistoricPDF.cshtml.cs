using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SALGAPortal.Pages
{
    public class DownloadHistoricPDFModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public String BlobName { get; set; }
        [BindProperty(SupportsGet = true)]
        public String ContainerName { get; set; }

        private IConfiguration _configuration { get; set; }
        public DownloadHistoricPDFModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //HistoricAssessments

        public IActionResult OnGet()
        {
            var storageKey = _configuration["AzureStorage:ConnectionString"];
            var containerName = _configuration["AzureStorage:"+ ContainerName];
            var containerClient = new BlobContainerClient(storageKey, containerName);
            var blobClient = containerClient.GetBlobClient(BlobName);
            MemoryStream stream = new MemoryStream();
            var blobDownloadInfo = blobClient.DownloadTo(stream);
            stream.Position = 0;
            return File(stream, "application/octet-stream", BlobName);
        }
    }
}
