using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class PreviousAssessments
    {
        [Parameter]
        public Municipality Municipality { get; set; }

        [Inject]
        IConfiguration _configuration { get; set; }

        public List<PreviousReportLinkViewModel> PastAssessmentsList { get; set; }

        public PreviousAssessments()
        {
            PastAssessmentsList = new List<PreviousReportLinkViewModel>();
        }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public void Update(Municipality municipality)
        {
            if (municipality!=null)
            {
                var currentYear = DateTime.Today.Year;
                var storageKey = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:HistoricAssessments"];

                PastAssessmentsList.Clear();
                var containerClient = new BlobContainerClient(storageKey, containerName);
                var containerURL = containerClient.Uri.ToString();
                var blobInfos = containerClient.GetBlobs(Azure.Storage.Blobs.Models.BlobTraits.Metadata).ToList();

                var thismunicipalityBlobs = blobInfos.Where(x =>x.Metadata.ContainsKey("Municipality")&& x.Metadata["Municipality"] == municipality.Name.Trim()).ToList();

                foreach (var blob in thismunicipalityBlobs)
                {
                    var blobYear = blob.Metadata["Year"];
                    if (blobYear != currentYear.ToString())
                    {
                        var prevAssessmentInfo = new PreviousReportLinkViewModel();
                        prevAssessmentInfo.Year = blobYear;
                        prevAssessmentInfo.Name = blob.Name;
                        prevAssessmentInfo.UrlLink = containerURL + "/" + blob.Name;
                        PastAssessmentsList.Add(prevAssessmentInfo);
                    }

                }

                StateHasChanged();
                
            }
        }

        protected override Task OnParametersSetAsync()
        {
            if (Municipality!=null)
                Update(Municipality);


            return base.OnParametersSetAsync();
        }

    }
}
