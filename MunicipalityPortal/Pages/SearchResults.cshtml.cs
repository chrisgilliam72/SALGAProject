using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MunicipalityPortal.ViewModels;
using SALGADBLib;

namespace MunicipalityPortal.Pages
{
    [Authorize]
    public class SearchResultsModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public String SearchString{ get; set; }

        private IAssessmentRepository _assessmentRepository;

        public List<DocumentSearchResultViewModel> SearchResuls { get; set; }
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IAuditingRepository _auditingRepository;
        private IDemographicsRepository _demographicsRepository;

        public SearchResultsModel(IAssessmentRepository assessmentRepository,
                                  IDemographicsRepository demographicsRepository,
                                  UserManager<IdentityUser> userManager,
                                  IConfiguration configuration,
                                  IAuditingRepository auditingRepository)
        {
            _assessmentRepository = assessmentRepository;
            _demographicsRepository = demographicsRepository;
            _userManager = userManager;
            _configuration = configuration;
            _auditingRepository = auditingRepository;

            SearchResuls = new List<DocumentSearchResultViewModel>();
        }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var demographics = await _demographicsRepository.GetDemographics(user);
                var answers = await _assessmentRepository.FindEvidenceDocuments(SearchString,demographics.Municipality.pkID);

                SearchResuls.AddRange(answers.Select(x => (new DocumentSearchResultViewModel
                {
                    DocumentName = x.OriginalFileName,
                    DocumentLink = x.FileUrl,
                    Municipality = x.Municipality.Name,
                    EvidenceID = x.pkID,
                    UserID= user.Email,
                    //HostURL= "http://localhost:7071/api/SALGAEvidenceAccess/"+ user.Email + "/"+ x.pkID
                })).ToList());
            }
        }

        public async Task<ActionResult> OnGetDownloadEvidenceFile(int evidenceID)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user!=null)
                {
         
                    var evidenceData = await _assessmentRepository.GetEvidence(evidenceID);
                    var audititem = new AuditEvent()
                    {
                        ItemName = evidenceData.OriginalFileName,
                        EventType = "Document Access",
                        Date = DateTime.Now,
                        UserIDString = user.Id,
                        ItemPath = evidenceData.Municipality.Name,
                        UserEmail = user.Email,
                    };
                    await _auditingRepository.AddCustomEvent(audititem);
                    var storageKey = _configuration["AzureStorage:ConnectionString"];
                    var containerName = _configuration["AzureStorage:EvidenceFiles"];
                    var containerClient = new BlobContainerClient(storageKey, containerName);
                    var blobClient = containerClient.GetBlobClient(evidenceData.BlobName);
                    MemoryStream stream = new MemoryStream();
                    var blobDownloadInfo = blobClient.DownloadTo(stream);
                    stream.Position = 0;
                    return File(stream, "application/octet-stream", evidenceData.BlobName);
                }

                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }
        public IActionResult OnPostViewDocument()
        {
            return RedirectToPage("SearchResults", "OnGetSearch", SearchString);
        }
    }
}
