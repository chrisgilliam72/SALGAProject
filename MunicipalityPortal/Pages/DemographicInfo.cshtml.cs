using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MunicipalityPortal.ViewModels;
using SALGADBLib;
using SALGAEvidenceRepository;
using SALGAWeb.ViewModels;

namespace SALGAWeb.Pages
{
    [Authorize]
    public class DemographicInfoModel : PageModel
    {

        [BindProperty()]
        public MunicipalityDemographics MunicipalityDemographics { get; set; }
        [BindProperty()]
        public IntervieweeDetails IntervieweeDetails { get; set; }
        [BindProperty()]
        public HRMDFunctionDemographics HRMDFunctionDemographics { get; set; }
        [BindProperty()]
        public IFormFile MunicipalOrganogram { get; set; }
        [BindProperty()]
        public IFormFile HRMDOrganogram { get; set; }
        [BindProperty()]
        public List<SelectListItem> JobTitles { get; set; }
        [BindProperty()]
        [Required]
        public String SelectedJobTitle { get; set; }
        [BindProperty()]
        public List<SelectListItem> ServiceOptions { get; set; }
        [BindProperty()]
        public String SelectedServiceOption { get; set; }
        public List<SelectListItem> Municipalities { get; set; }
        [BindProperty()]
        [Required]
        public String SelectedMunicipality { get; set; }
        [BindProperty()]
        public String MunicipalityCatagory { get; set; }
        [BindProperty()]
        [Column(TypeName = "decimal(7, 2)")]
        [RegularExpression(@"^\d+\,\d{2}$", ErrorMessage ="Value must be a number with 2 decimal places")]
        public decimal TotalPayRollValue { get; set; }
        public List<MunicipalityAndCategory> MunicipalityAndCategories {get;set;}

        public bool IsShowingCapturer { get; set; }

        public String UserDetailsCaption { get; set; }

        public List<SelectListItem> MunicipalCategories { get; set; }
        [BindProperty()]
        public List<SeniorManagerDemographicsViewmodel> SeniorManagerList { get; set; }
        private IDemographicsRepository _demographicsRepository;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IAzureRepository _evidenceRepository;
        public DemographicInfoModel(UserManager<IdentityUser> userManager, IConfiguration configuration,
                                    IDemographicsRepository demographicsRepository, IAzureRepository evidenceRepository)
        {
            JobTitles = new List<SelectListItem>();
            ServiceOptions = new List<SelectListItem>();
            MunicipalCategories = new List<SelectListItem>();
            SeniorManagerList = new List<SeniorManagerDemographicsViewmodel>();
            _demographicsRepository = demographicsRepository;
            SelectedJobTitle = "";
            SelectedMunicipality = "";
            _userManager = userManager;
            _configuration = configuration;
            _evidenceRepository = evidenceRepository;

        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                await LoadDropdownItems();

                IntervieweeDetails = new IntervieweeDetails();
                IntervieweeDetails.InterviewDate = DateTime.Today;

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {

                    var isApprover = await _userManager.IsInRoleAsync(user, "Approver");
                    var isCapturer = await _userManager.IsInRoleAsync(user, "Capturer");
                    IntervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(user);
                    if (IntervieweeDetails != null)
                    {
                        SelectedJobTitle = IntervieweeDetails.JobTitle.Title;
                        SelectedMunicipality = IntervieweeDetails.Municipality.Name;
                        MunicipalityCatagory = IntervieweeDetails.Municipality.MunicipalCatagory.Catagory;

                        if (isCapturer)
                        {

                            MunicipalityDemographics = await _demographicsRepository.GetDemographics(user);
                            if (MunicipalityDemographics == null)
                                MunicipalityDemographics = new MunicipalityDemographics();


                            HRMDFunctionDemographics = await _demographicsRepository.GetHRMDDemographics(user);
                            if (HRMDFunctionDemographics == null)
                                HRMDFunctionDemographics = new HRMDFunctionDemographics();

                            IsShowingCapturer = true;
                        }
                        else
                        {
                            MunicipalityDemographics = await _demographicsRepository.GetDemographics(IntervieweeDetails.Municipality);
                            HRMDFunctionDemographics = await _demographicsRepository.GetHRMDDemographics(IntervieweeDetails.Municipality);

                            IsShowingCapturer = false;
                        }

                        if (MunicipalityDemographics!=null)
                        {
                            TotalPayRollValue = Convert.ToDecimal(MunicipalityDemographics.TotalMonthlyPayroll);

                        }

                    }

                    SeniorManagerList.Clear();
                    var seniorPositions = (await _demographicsRepository.GetSeniorManagerPositions(MunicipalityDemographics)).ToList();
                    if (seniorPositions==null || seniorPositions.Count==0)
                    {
                        var seniorManagers = (await _demographicsRepository.GetSeniorManagers()).ToList();
                        SeniorManagerList.AddRange(seniorManagers.Select(x => (SeniorManagerDemographicsViewmodel)x).ToList());
                    }
                    else
                        SeniorManagerList.AddRange(seniorPositions.Select(x => (SeniorManagerDemographicsViewmodel)x).ToList());

                    SeniorManagerList = SeniorManagerList.OrderBy(x => x.DisplayOrder).ToList();
                    return Page();
                }

                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }


        public async Task<IActionResult> OnPost()
        {
            try
            {

                var storageKey = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:EvidenceFiles"];

                var user = await _userManager.GetUserAsync(User);
                if (user!=null)
                {

                    var isApprover = await _userManager.IsInRoleAsync(user, "Approver");
                    var isCapturer = await _userManager.IsInRoleAsync(user, "Capturer");

                    var dbJobTitles = await _demographicsRepository.GetJobTitles();
                    var dbMunipalities = await _demographicsRepository.GetMunicipalities();
                    var seniorManagers = await _demographicsRepository.GetSeniorManagers();

                    IntervieweeDetails.JobTitle = dbJobTitles.FirstOrDefault(x => x.Title == SelectedJobTitle);
                    IntervieweeDetails.Municipality = dbMunipalities.FirstOrDefault(x => x.Name == SelectedMunicipality);
                    IntervieweeDetails.User = user;
                    IntervieweeDetails.Onboarded = true;
                    if (isCapturer)
                        MunicipalityDemographics.Capturer = user;
                    if(isApprover)
                        MunicipalityDemographics.Approver = user;

                    HRMDFunctionDemographics.User = user;
                    MunicipalityDemographics.Municipality = IntervieweeDetails.Municipality;
                    HRMDFunctionDemographics.Municipality = IntervieweeDetails.Municipality;
                    if (SelectedServiceOption == "Corporate Service")
                        HRMDFunctionDemographics.CorporateService = true;
                    if (HRMDFunctionDemographics != null)
                    {
                        _evidenceRepository.SetConnectionString(storageKey);
                        if (HRMDOrganogram!=null)
                        {
                            var organogram = new MunicipalityOrganogram()
                            {
                                OriginalFileName = HRMDOrganogram.FileName,
                                BlobName = SelectedMunicipality + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + "_" + HRMDOrganogram.FileName,
                                ContainerName = containerName
                            };

                            using (var memStream = new MemoryStream())
                            {
                                HRMDOrganogram.CopyTo(memStream);
                                organogram.URL = await _evidenceRepository.AddFile(containerName, organogram.BlobName, memStream);

                            }

                            HRMDFunctionDemographics.MunicipalityOrganogram = organogram;
                        }                    

                    }

                    //var tmpPayRollStringValue = PayRollStringValue.Replace(".", ",");
                    MunicipalityDemographics.TotalMonthlyPayroll = Convert.ToDouble(TotalPayRollValue);
                    SelectedJobTitle = IntervieweeDetails.JobTitle.Title;
                    SelectedMunicipality = IntervieweeDetails.Municipality.Name;
                    MunicipalityCatagory = IntervieweeDetails.Municipality.MunicipalCatagory.Catagory;

                    if (ModelState.IsValid)
                    {

                        await _demographicsRepository.SaveInterviewDetails(IntervieweeDetails);
                        await _demographicsRepository.SaveMunicipalityDemographics(MunicipalityDemographics);
                        await _demographicsRepository.SaveHRMDDemographics(HRMDFunctionDemographics);

                        var listPositions = new List<SeniorManagerPosition>();
                        foreach( var managerModel in SeniorManagerList)
                        {
                            var seniorManPos = new SeniorManagerPosition()
                            {
                                pkID = managerModel.FilledPositionID,
                                SeniorManager = seniorManagers.FirstOrDefault(x => x.pkID == managerModel.ManagerID),
                                Name= managerModel.Filledby!=null ? managerModel.Filledby : "",    
                                AppointmentDate=managerModel.AppointmentDate,
                                MunicipalityDemographics = MunicipalityDemographics,
                                PortfolioDisplayValue= managerModel.Portfolio
                            };
                            listPositions.Add(seniorManPos);
                        }

                        await _demographicsRepository.UpdateSeniorManagerPositions(listPositions);
                    }
                    else
                    {
                        await LoadDropdownItems();
                        return Page();
                    }
                }

                return RedirectToPage("/QuestionnaireStart");
            }
           catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return RedirectToPage("/Error", new { ExceptionString = ex.Message });
                else
                    return RedirectToPage("/Error", new { ExceptionString = ex.InnerException });
            }

        }

        private async Task LoadDropdownItems()
        {
            var jobTitles = await _demographicsRepository.GetJobTitles();
            var serviceOptions = await _demographicsRepository.GetServiceOptions();
            var dbMunicipalities = (await _demographicsRepository.GetMunicipalities()).ToList();
            MunicipalityAndCategories = dbMunicipalities.Select(x => new MunicipalityAndCategory { Municipality = x.Name, Category = x.MunicipalCatagory.Catagory }).ToList();

            Municipalities = dbMunicipalities.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).OrderBy(x => x.Text).ToList();
            JobTitles = jobTitles.Select(x => new SelectListItem { Value = x.Title, Text = x.Title }).ToList();
            ServiceOptions = serviceOptions.Select(x => new SelectListItem { Value = x.DisplayString, Text = x.DisplayString }).ToList();
 
        }
    }
}
