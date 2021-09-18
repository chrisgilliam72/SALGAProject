using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MunicipalityPortal.ViewModels;
using SALGADBLib;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using SALGAWeb.ViewModels;

namespace SALGAWeb.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private IAssessmentRepository _assessmentRepository;
        private IDemographicsRepository _demographicsRepository;
        private UserManager<IdentityUser> _userManager;


        public int OverrallLevel { get; set; }

        public String TypeMunicipality { get; set; }
        public String NoPeopleEmployed { get; set; }
        public String NoHRMDStaff { get; set; }
        public double AverageSalaryPP { get; set; }
        public decimal HRRatio { get; set; }
        public String MunicipalityName { get; set; }
        public String MunicipalityType { get; set; }

        public int NoPerm54A56 { get; set; }
        public int NoFixedTerm54A56 { get; set; }
        public int NoPermNon54A56 { get; set; }
        public int NoFixedTermNon54A56 { get; set; }
        public int NoOther { get; set; }

        public String ExecutiveMayor { get; set; }
        public String CityManager { get; set; }
        public String DirectorHR { get; set; }
        public String COO { get; set; }


        public MaturityLevelDashboardViewModel MaturityLevelDashboardViewModel { get; set; }

        public HRMDFunctionDemographics HRMDFunctionDemographics { get; set; }
        [BindProperty(SupportsGet = true)]
        public MunicipalityDemographics MunicipalityDemographics { get; set; }

        public List<DashboardManagerItemViewModel> ManagerItems { get; set; }

        public DashboardModel(IDemographicsRepository demographicsRepository,
                              IAssessmentRepository assessmentRepository, UserManager<IdentityUser> userManager)
        {

            _assessmentRepository = assessmentRepository;
            _demographicsRepository = demographicsRepository;
            _userManager = userManager;
            ManagerItems = new List<DashboardManagerItemViewModel>();
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser != null)
                {

                    var questionnairePages = (await _assessmentRepository.GetPageInfo()).ToList();
                    var questionnaireQuestions = (await _assessmentRepository.GetQuestions()).ToList();
                    var responseTypes = (await _assessmentRepository.GetResponseTypes()).ToList();
                    var responseYes = responseTypes.FirstOrDefault(x => x.ResponseType == "Yes");

                    var isApprover = await _userManager.IsInRoleAsync(identityUser, "Approver");
                    List<QuestionnaireQuestionAnswer> questionnaireAnswers = null;
                    var demographics = await _demographicsRepository.GetDemographics(identityUser);
                    var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                    if (config!=null)
                    {
                        if (isApprover)
                        {
                            if (demographics != null)
                            {
                                var tracking = await _assessmentRepository.GetAssessmentTracking(demographics.Capturer, config.CurrentYear);
                                var approverDetails = await _demographicsRepository.GetIntervieweeDetails(identityUser);
                                HRMDFunctionDemographics = await _demographicsRepository.GetHRMDDemographics(approverDetails.Municipality);
                                MunicipalityDemographics = await _demographicsRepository.GetDemographics(approverDetails.Municipality);
                                questionnaireAnswers = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(tracking)).ToList();
                            }

                        }
                        else
                        {
                            var tracking = await _assessmentRepository.GetAssessmentTracking(identityUser, config.CurrentYear);
                            HRMDFunctionDemographics = await _demographicsRepository.GetHRMDDemographics(identityUser);
                            MunicipalityDemographics = await _demographicsRepository.GetDemographics(identityUser);
                            questionnaireAnswers = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(tracking)).ToList();
                        }

                        if (HRMDFunctionDemographics == null)
                            HRMDFunctionDemographics = new HRMDFunctionDemographics();

                        IntervieweeDetails intervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(identityUser);
                        MunicipalityName = intervieweeDetails.Municipality.Name;
                        MunicipalityType = intervieweeDetails.Municipality.MunicipalCatagory.Catagory;

                        if (MunicipalityDemographics != null)
                        {
                            NoPerm54A56 = MunicipalityDemographics.NoPerm54A56;
                            NoFixedTerm54A56 = MunicipalityDemographics.NoFixedTerm54A56;
                            NoPermNon54A56 = MunicipalityDemographics.NoPermNon54A56;
                            NoFixedTermNon54A56 = MunicipalityDemographics.NoFixedTermNon54A56;
                            NoOther = MunicipalityDemographics.NoOther;
                            if (HRMDFunctionDemographics.NoPeople > 0)
                                HRRatio = Math.Round((decimal)(MunicipalityDemographics.NoEmployees / HRMDFunctionDemographics.NoPeople), 2);
                            if (MunicipalityDemographics.NoEmployees > 0)
                                AverageSalaryPP = Math.Round((double)(MunicipalityDemographics.TotalMonthlyPayroll / MunicipalityDemographics.NoEmployees), 2);

                        }

                        else
                            MunicipalityDemographics = new MunicipalityDemographics();


                        MaturityLevelDashboardViewModel = await MunicipalityDashboardReport.LoadMunicipalityReport(intervieweeDetails.Municipality, config.CurrentYear, _assessmentRepository);

                        var ManagerList = await _demographicsRepository.GetSeniorManagerPositions(MunicipalityDemographics);
                        foreach (var manager in ManagerList)
                        {
                            if (!String.IsNullOrEmpty(manager.Name))
                            {
                                var managerItem = new DashboardManagerItemViewModel()
                                {
                                    Person = manager.Name,
                                    Portfolio = manager.PortfolioDisplayValue,
                                    AppointedDate = manager.AppointmentDate.ToShortDateString()
                                };

                                ManagerItems.Add(managerItem);
                            }

                        }


                        return Page();
                    }
                    else
                        return RedirectToPage("/AssessmenNotOpen");
                }


                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }

        }
    }
}

