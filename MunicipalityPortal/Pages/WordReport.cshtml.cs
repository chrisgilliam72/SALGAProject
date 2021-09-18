using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MunicipalityPortal.ViewModels;
using SALGADBLib;
using SALGAEvidenceRepository;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace MunicipalityPortal.Pages
{
    [Authorize]
    public class WordReportModel : PageModel
    {
        public Municipality Municipality { get; set; }
        public HRMDTableViewModel HRMDTableViewModel { get; set; }
        public MaturityLevelDashboardViewModel MaturityLevelDashboardViewModel { get; set; }
        public List<PreviousReportLinkViewModel> PastAssessmentsList { get; set; }
        public String TechnologyResponse { get; set; }

        public OverallMaturityLevels OverallMaturityLevels
        {
            get
            {
                return MaturityLevelDashboardViewModel.OverallMaturityLevels;
            }
        }
        private IDemographicsRepository _demographicsRepository;
        private IAssessmentRepository _assessmentRepository;
        private IAzureRepository _azureRepository;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public WordReportModel(IDemographicsRepository demographicsRepository, IAssessmentRepository assessmentRepository,
                                IAzureRepository azureRepository,IConfiguration configuration, UserManager<IdentityUser> userManager,
                                IWebHostEnvironment hostingEnvironment)
        {
            _demographicsRepository = demographicsRepository;
            _assessmentRepository = assessmentRepository;
            _azureRepository = azureRepository;
            _userManager = userManager;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            PastAssessmentsList = new List<PreviousReportLinkViewModel>();
        }

        public async Task<IActionResult> OnGet()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            var storageKey = _configuration["AzureStorage:ConnectionString"];
            var containerName = _configuration["AzureStorage:HistoricAssessments"];
            int currentYear = DateTime.Now.Year;
            if (identityUser != null)
            {
                IntervieweeDetails intervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(identityUser);
                Municipality = intervieweeDetails.Municipality;
                var config = await _assessmentRepository.GetAssessmentConfig(Municipality);
                if (config!=null)
                {
                    TechnologyResponse = await _assessmentRepository.GetTechnologyInUse(Municipality, config.CurrentYear);
                    HRMDTableViewModel = await DemographicsReport.LoadDemographicsTableReport(Municipality, _demographicsRepository);

                    MaturityLevelDashboardViewModel = await MunicipalityDashboardReport.LoadMunicipalityReport(Municipality, config.CurrentYear, _assessmentRepository);
                    MaturityLevelDashboardViewModel.SectionsMatrix.Build(MaturityLevelDashboardViewModel.GroupedLevelSections);

                    var containerClient = new BlobContainerClient(storageKey, containerName);
                    var containerURL = containerClient.Uri.ToString();
                    var blobInfos = containerClient.GetBlobs(Azure.Storage.Blobs.Models.BlobTraits.Metadata).ToList();
                    var thismunicipalityBlobs = blobInfos.Where(x => x.Metadata.ContainsKey("Municipality") && x.Metadata["Municipality"] == Municipality.Name.Trim()).ToList();
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
                    return Page();
                }
                else
                    return RedirectToPage("/AssessmenNotOpen");

                //var corrpt= blobMetadata.Where(x => x.ContainsKey("Municipality") == false).ToList();
            }

            return RedirectToPage("/Login");
        }
        //https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/webkit

        public async Task<IActionResult>OnPostExportToPDF()
        {
            try
            {
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser != null)
                {
                    var storageKey = _configuration["AzureStorage:ConnectionString"];
                    var containerName = _configuration["AzureStorage:HistoricAssessments"];

                    var currentYear = DateTime.Today.Year;
                    var blobMetaData = new Dictionary<string, string>();
                    IntervieweeDetails intervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(identityUser);
                    Municipality = intervieweeDetails.Municipality;
                    string cookieName = ".AspNetCore.Identity.Application";
                    string cookieValue = string.Empty;
                    if (Request.Cookies[cookieName] != null)
                    {
                        cookieValue = Request.Cookies[cookieName];
                    }
                    //Initialize HTML converter
                    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
                    //WebKit converter settings
                    WebKitConverterSettings webKitSettings = new WebKitConverterSettings();
                    webKitSettings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinaries");
                    //Assign the WebKit binaries path
                    webKitSettings.EnableJavaScript = true;
                    webKitSettings.EnableToc = true;
                    webKitSettings.EnableHyperLink = true;
                    webKitSettings.AdditionalDelay = 5000;
                    //Add cookies as name and value pair
                    webKitSettings.Cookies.Add(cookieName, cookieValue);
                    //Assign the WebKit settings
                    htmlConverter.ConverterSettings = webKitSettings;
                    //Convert URL to PDF
                    var pageLink = Url.PageLink("WordReportPrinting");
                    //PdfDocument document = htmlConverter.Convert(reportLink);
                    //PdfDocument document = htmlConverter.Convert("www.news24.com");
                    PdfDocument document = htmlConverter.Convert(pageLink);
                    //Save the document into stream.
                    MemoryStream stream = new MemoryStream();

                    document.Save(stream);

                    stream.Position = 0;

                    //Close the document.
                    document.Close(true);

                    blobMetaData.Add("Municipality", Municipality.Name.Replace(" ","%20"));
                    blobMetaData.Add("Year", currentYear.ToString());
                    _azureRepository.SetConnectionString(storageKey);
                    await _azureRepository.AddFile(containerName, Municipality.Name + " MuniHRPlusReport.pdf", stream);
                    await _azureRepository.UpdateMetaData(containerName, Municipality.Name + " MuniHRPlusReport.pdf",blobMetaData);
                    stream.Position = 0;
                    //Creates a FileContentResult object by using the file contents, content type, and file name.
                    return File(stream, "application/pdf", Municipality.Name + " Municipal HR Pulse Report.pdf");

                }

                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostDownloadHistoricReport(String blobName)
        {
            try
            {
                var storageKey = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:HistoricAssessments"];
                var containerClient = new BlobContainerClient(storageKey, containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                MemoryStream stream = new MemoryStream();
                var blobDownloadInfo = blobClient.DownloadTo(stream);
                stream.Position = 0;
                return File(stream, "application/pdf", blobName);
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }
    }
}
