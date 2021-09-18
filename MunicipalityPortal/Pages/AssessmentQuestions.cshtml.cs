using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MunicipalityPortal.ViewModels;
using SALGADBLib;
using SALGAWeb.ViewModels;

namespace SALGAWeb.Pages
{
    [Authorize]
    public class AssessmentQuestionsModel : PageModel
    {
        [BindProperty()]
        public List<QuestionnaireCategoryViewModel> ListSections { get; set; }

        private IAssessmentRepository _assessmentRepository;
        private IDemographicsRepository _demographicsRepository;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IAuditingRepository _auditingRepository;
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }

        public bool HasRejectSections
        {
            get
            {
                return (ListSections.Count(x => x.IsRejected) > 0);
            }
        }

        public bool CanEdit
        {
            get
            {
                if (!ApprovalMode)
                    return (!IsApproved && !IsSubmitted);
                else
                {
                    return IsSubmitted && !IsApproved;
                }
          
            }

        }
        public bool CanSubmit
        {
            get
            {
                return (ListSections.Count(x => x.IsComplete == false) == 0);
            }
        }
        [BindProperty()]

        public bool ApprovalMode { get; set; }


        public AssessmentQuestionsModel(IDemographicsRepository demographicsRepository, IAssessmentRepository assessmentRepository,
                                        IConfiguration configuration,UserManager<IdentityUser> userManager, IAuditingRepository auditingRepository)
        {

            _demographicsRepository = demographicsRepository;
            _assessmentRepository = assessmentRepository;
            _userManager = userManager;
            _configuration = configuration;
            _auditingRepository = auditingRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var intervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(user);

                    var dbListPages = (await _assessmentRepository.GetPageInfo()).ToList();
                    var dbListQuestions = (await _assessmentRepository.GetQuestions()).ToList();
                    List<QuestionnaireQuestionAnswer> lstQestions = null;
                    AssessmentTracking assessmentTracking = null;

                    ApprovalMode = await _userManager.IsInRoleAsync(user, "Approver");
                    var demographics = await _demographicsRepository.GetDemographics(user);
                    if (demographics != null)
                    {
                        var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                        if (config != null)
                        {
                            if (ApprovalMode)
                            {
                                if (demographics != null)
                                    assessmentTracking = await _assessmentRepository.GetAssessmentTracking(demographics.Capturer, config.CurrentYear);
                            }
                            else
                            {

                                assessmentTracking = await _assessmentRepository.GetAssessmentTracking(user, config.CurrentYear);
                                if (assessmentTracking == null)
                                    assessmentTracking = await _assessmentRepository.CreateAssessmentTracking(user, intervieweeDetails.Municipality, config.CurrentYear);
                            }

                            lstQestions = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(assessmentTracking)).ToList();
                            IsApproved = assessmentTracking != null && assessmentTracking.IsApproved;
                            IsSubmitted = assessmentTracking != null && assessmentTracking.IsSubmitted;
                            ListSections = dbListPages.Select(x => (QuestionnaireCategoryViewModel)x).ToList();

                            if (lstQestions != null && lstQestions.Count > 0)
                            {
                                var grpedQuestions = lstQestions.GroupBy(x => x.Question.PageNo).ToList();
                                foreach (var grpedQ in grpedQuestions)
                                {
                                    var questionsAnswered = grpedQ.Where(x => x != null).ToList();
                                    var section = ListSections.FirstOrDefault(x => x.SectionNo == grpedQ.Key);
                                    if (section != null)
                                    {
                                        double maxQuestions = dbListQuestions.Where(x => x.PageNo == grpedQ.Key).Count();
                                        double answerdQuestions = grpedQ.Where(x => (x.ResponseType != null && x.ResponseType.ResponseType != "Rejected") && (x.CustomResponse != "Rejected")).Count();
                                        section.Percent = maxQuestions != 0 ? Convert.ToInt32((answerdQuestions / maxQuestions) * 100) : 0;
                                        section.IsRejected = grpedQ.Where(x => (x.ResponseType != null && x.ResponseType.ResponseType == "Rejected") || (x.CustomResponse == "Rejected")).Count() > 0;
                                    }

                                }
                            }

                            return Page();
                        }
                        else
                            return RedirectToPage("/AssessmenNotOpen");
                    }
                    else
                        RedirectToPage("/DemographicInfo");

                }

                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new { ExceptionString = ex.Message });
            }

       
        }


        public async Task<IActionResult> OnPostReject()
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
                var user = await _userManager.GetUserAsync(User);
     
                var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
                var demographics = await _demographicsRepository.GetDemographics(user);
                var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                var tracking = await _assessmentRepository.GetAssessmentTracking(demographics.Capturer, config.CurrentYear);
                var pageInfo = await _assessmentRepository.GetPageInfo();
                var loginLink = Url.PageLink("Login");
                var lstQuestions = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(tracking)).ToList();
                var rejectedQuestions = lstQuestions.Where(x => x.ResponseType.ResponseType == "Rejected" || x.CustomResponse=="Rejected").GroupBy(x => x.Question.PageNo).ToList();
                String rejectedQuestionString = "";
                var mailHelp = new SendMail(host, port, username, password, enable);


                foreach (var rejectedQuestion in rejectedQuestions)
                {
                    var sectionTitle = pageInfo.First(x => x.PageNo == rejectedQuestion.Key).Title;
                    rejectedQuestionString += @"<h3 style = ""margin-top: 40px;"">" + sectionTitle + @"</h3>";

                    foreach (var comment in rejectedQuestion)
                    {
                        rejectedQuestionString += @"<p style = ""color: red"" >" + "Q." + comment.Question.QuestionNo + " " + comment.ApproverComments + @"</p>";

                    }

                }

                mailHelp.SendHTMLAsync(username, user.UserName, new List<string> { "chrisg@daita.co.za", demographics.Capturer.Email }, demographics.Capturer.Email, "Municipal HR Pulse Portal assessment rejected",
                rejectedQuestionString, rootDir + @"\wwwroot\EmailTemplates\index-assessment-rejected.html", loginLink, false);

                if (tracking != null)
                {
                    tracking.IsApproved = false;
                    tracking.IsSubmitted = false;
                    await _assessmentRepository.UpdateAssesmentTracking(tracking);
                    await _auditingRepository.AddAssessmentRejectedEvent(user, demographics.Municipality.Name);
                }

                return RedirectToPage("/Dashboard");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostSubmitted()
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
                var user = await _userManager.GetUserAsync(User);

                var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
                var demographics = await _demographicsRepository.GetDemographics(user);
                var intervieweeDetailsLst = await _demographicsRepository.GetIntervieweeDetails(demographics.Municipality);
                var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                var capturer = demographics.Capturer;

                var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(user, config.CurrentYear);
                if (assessmentTracking != null)
                {
                    assessmentTracking.IsApproved = false;
                    assessmentTracking.IsSubmitted = true;
                    await _assessmentRepository.UpdateAssesmentTracking(assessmentTracking);
                }

                await _auditingRepository.AddAssessmentSubmittedEvent(user, demographics.Municipality.Name);

                if (!assessmentTracking.IsApproved)
                {
                    foreach (var details in intervieweeDetailsLst)
                    {
                        bool isApprover = await _userManager.IsInRoleAsync(details.User, "Approver");
                        if (isApprover)
                        {
                            var name = details.FirstName + " " + details.LastName;
                            var mailHelp = new SendMail(host, port, username, password, enable);
                            mailHelp.SendHTMLAsync(username, details.Email  , new List<string> { "chrisg@daita.co.za", details.Email }, user.UserName, "Municipal HR Pulse Portal assessment review required", "",
                                 rootDir + @"\wwwroot\EmailTemplates\index-review-assessment.html", "", false);

                        }
                    }
                }

                return RedirectToPage("/Dashboard");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }

        }

        public async Task<IActionResult> OnPostApprove()
        {
            try
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
                var user = await _userManager.GetUserAsync(User);
                var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
                var loginLink = Url.PageLink("Login");
                var salgaEmail =_configuration["SALGAReviewEmailAddress"];
                if (user != null)
                {
                    if (ApprovalMode)
                    {
                        var demographics = await _demographicsRepository.GetDemographics(user);
                        var mailHelp = new SendMail(host, port, username, password, enable);

                        var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                        mailHelp.SendHTMLAsync(username, user.UserName, new List<string> { "chrisg@daita.co.za", salgaEmail }, demographics.Municipality.Name, "Municipal HR Pulse Portal assessment ready for review", "",
                        rootDir + @"\wwwroot\EmailTemplates\salga-review-assessment.html",
                        _configuration["SALGAPortalUrl"], false);

                        var tracking = await _assessmentRepository.GetAssessmentTracking(demographics.Capturer, config.CurrentYear);
                        if (tracking != null)
                        {
                            tracking.IsApproved = true;
                            tracking.ApprovedDate = DateTime.Now;
                            tracking.SALGARejected = false;
                            tracking.Approver = user;
                            await _assessmentRepository.UpdateAssesmentTracking(tracking);
                            await _auditingRepository.AddAssessmentApprovedEvent(user,demographics.Municipality.Name);

                        }


                    }

                }

                return RedirectToPage("/Dashboard");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }

        }


    }
}

