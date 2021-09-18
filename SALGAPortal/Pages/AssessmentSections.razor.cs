using MailHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AssessmentSections
    {
             [Inject]
        IAssessmentRepository _assessmentRepository { get; set; }
        [Inject]
        IConfiguration _configuration { get; set; }
        public List<AssessmentSectionViewModelcs> Sections { get; set; }

        public List<AssessmentSectionsReviewViewModel> ReviewViewModels { get; set; }

        
        private List<QuestionnaireResponseType> ResponseTypes { get; set; }
        private bool IsApproved { get; set; }
        private bool IsRejected { get; set; }
        private bool IsReady { get; set; }
        public Municipality Municipality { get; set; }

        private String SelectedYear { get; set; }
        private List<String> YearList { get; set; }

        private bool ShowingCurrentYear { get; set; }



        public AssessmentSections()
        {
            Sections = new List<AssessmentSectionViewModelcs>();
            ReviewViewModels = new List<AssessmentSectionsReviewViewModel>();
            YearList = new List<string>();
        }

        private async Task UpdateQuestionData(AssessmentTracking assessmentTracking)
        {
            Sections.Clear();
            ReviewViewModels.Clear();
            var categories = await _assessmentRepository.GetPageInfo();
            var questions = await _assessmentRepository.GetQuestions();
            var answers = await _assessmentRepository.GetQuestionnaireQuestionAnswers(assessmentTracking);
            var customResponseTypes = await _assessmentRepository.GetCustomResponseTypes();
            foreach (var category in categories)
            {
                int pageNo = category.PageNo;
                int noQuestionsAnswered = answers.Where(x => x.Question.PageNo == category.PageNo && x.ResponseType.ResponseType.ToLower()!="rejected").Count();
                int noQuestions = questions.Where(x => x.PageNo == category.PageNo).Count();
                var sectionVM = new AssessmentSectionViewModelcs();
                sectionVM.PercentComplete = Convert.ToInt32(((double)noQuestionsAnswered / (double)noQuestions) * 100);
                sectionVM.Name = category.Title;
                sectionVM.IconPath = category.IconPath;
                Sections.Add(sectionVM);

                var reviewSectionViewModel = new AssessmentSectionsReviewViewModel();
                var listQuestions = (await _assessmentRepository.GetQuestions()).ToList();
                var tmpResponseTypes = (await _assessmentRepository.GetResponseTypes()).ToList();
                var questionnaireResponseTypes = tmpResponseTypes.Where(x=>x.ResponseType.ToLower()!="custom");
                ResponseTypes = questionnaireResponseTypes.ToList();
                reviewSectionViewModel.ListQuestionsAnswers = listQuestions.Where(x => x.PageNo == pageNo).Select(x => new QuestionnaireQuestionViewModel(x)).ToList();

                var customResponseQues = reviewSectionViewModel.ListQuestionsAnswers.Where(x => x.HasCustomResponses == true);
                foreach (var question in customResponseQues)
                {
                    var lstResponses = customResponseTypes.Where(x => x.QuestionNo == question.QuestionNo && x.VisibleToApprover).ToList();
                    question.CustomReponses = lstResponses;
                }

                foreach (var questonAnswer in reviewSectionViewModel.ListQuestionsAnswers)
                {
                    var answer = answers.FirstOrDefault(x => x.Question.QuestionNo == questonAnswer.QuestionNo);
                    if (answer != null)
                    {
                        questonAnswer.Comment = answer.Comments;
                        questonAnswer.FileName = answer.AnswerEvidence != null ? answer.AnswerEvidence.OriginalFileName : "";
                        questonAnswer.AnswerType = answer.ResponseType != null ? answer.ResponseType.ResponseType : "";
                        questonAnswer.FilePath = answer.AnswerEvidence != null ? "/DownloadEvidenceFile?BlobName=" + answer.AnswerEvidence.BlobName : "";
                        questonAnswer.SALGAComment = answer.SALGAComments != null ? answer.SALGAComments : "";
                        questonAnswer.AnswerID = answer.pkID;

                        if (answer.Question.HasCustomResponsesTypes)
                            questonAnswer.AnswerType = answer.CustomResponse;
                    }


                }

                ReviewViewModels.Add(reviewSectionViewModel);
            }
        }

        public async Task Update(Municipality municipality)
        {

            YearList.Clear();
            Municipality = municipality;
            if (Municipality!=null)
            {
                var config = await _assessmentRepository.GetAssessmentConfig(Municipality);
                if (config != null)
                {

                    var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(municipality, config.CurrentYear);
                    if (assessmentTracking != null)
                    {
                        if (assessmentTracking.SALGAApproved)
                            IsApproved = true;
                        else if (assessmentTracking.SALGARejected)
                            IsRejected = true;
                        else if (assessmentTracking.IsApproved)
                            IsReady = true;

                        var trackings = await _assessmentRepository.GetAssessmentTrackings(Municipality);
                        var years = trackings.Select(x => x.AuditYear).OrderByDescending(x => x).ToList();
                        foreach (var year in years)
                        {
                            if (year == config.CurrentYear)
                            {
                                SelectedYear = year.ToString() + " (current)";
                                YearList.Add(SelectedYear);
                            }

                            else
                                YearList.Add(year.ToString());
                        }

                        await UpdateQuestionData(assessmentTracking);

                        ShowingCurrentYear = true;
             
                    }

                }
            }
            else
            {
                Sections.Clear();
                ReviewViewModels.Clear();
            }
            StateHasChanged();

        }



        public async Task OnSubmitAssessment()
        {
            IsRejected = true;
            IsReady = false;
            IsApproved = true;
            var host = _configuration["Gmail:Host"];
            var port = int.Parse(_configuration["Gmail:Port"]);
            var username = _configuration["Gmail:Username"];
            var password = _configuration["Gmail:Password"];
            var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
            var pageInfo = await _assessmentRepository.GetPageInfo();
            var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var loginLink = _configuration["MunicipalPortalURL"];
            var config = await _assessmentRepository.GetAssessmentConfig(Municipality);
            if (config!=null)
            {
                var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(Municipality, config.CurrentYear);
                if (assessmentTracking!=null)
                {
                    assessmentTracking.SALGARejected = false;
                    assessmentTracking.SALGAApproved = true;
                    assessmentTracking.IsApproved = true;
                    assessmentTracking.SALGAReviewDate = DateTime.Now;
                    await _assessmentRepository.UpdateAssesmentTracking(assessmentTracking);

                    var mailHelp = new SendMail(host, port, username, password, enable);
                    mailHelp.SendHTMLAsync(username, null, new List<string> { assessmentTracking.User.Email }, null, "MunicipalHRPulse Portal assessment approved by SALGA",
                        "", rootDir + @"\wwwroot\EmailTemplates\assessment-approved.html", loginLink, false);

                    StateHasChanged();
                }
            }
        }



        public async Task OnRejectAssessment()
        {
            IsRejected = true;
            IsApproved = false;
            IsReady = false;
            var host = _configuration["Gmail:Host"];
            var port = int.Parse(_configuration["Gmail:Port"]);
            var username = _configuration["Gmail:Username"];
            var password = _configuration["Gmail:Password"];
            var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
            var pageInfo = await _assessmentRepository.GetPageInfo();
            var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var config = await _assessmentRepository.GetAssessmentConfig(Municipality);
            if (config != null)
            {
                var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(Municipality, config.CurrentYear);
                if (assessmentTracking!=null)
                {
                    assessmentTracking.SALGAApproved = false;
                    assessmentTracking.SALGARejected = true;
                    assessmentTracking.IsApproved = false;
                    assessmentTracking.IsSubmitted = false;
                    assessmentTracking.SALGAReviewDate = DateTime.Now;
                    await _assessmentRepository.UpdateAssesmentTracking(assessmentTracking);
                    String rejectedQuestionString = "";
                    var lstQuestions = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(assessmentTracking)).ToList();
                    var rejectedQuestions = lstQuestions.Where(x => x.ResponseType.ResponseType.ToLower() == "rejected").GroupBy(x => x.Question.PageNo).ToList();
                    //var rejectedQuestions = lstQuestions.Where(x => !String.IsNullOrEmpty(x.SALGAComments)).GroupBy(x => x.Question.PageNo).ToList();
                    var loginLink = _configuration["MunicipalPortalURL"];

                    foreach (var rejectedQuestion in rejectedQuestions)
                    {
                        var sectionTitle = pageInfo.First(x => x.PageNo == rejectedQuestion.Key).Title;
                        rejectedQuestionString += @"<h3 style = ""margin-top: 40px;"">" + sectionTitle + @"</h3>";

                        foreach (var comment in rejectedQuestion)
                        {
                            rejectedQuestionString += @"<p style = ""color: red"" >" + "Q." + comment.Question.QuestionNo + " " + comment.SALGAComments + @"</p>";

                        }

                    }

                    var mailHelp = new SendMail(host, port, username, password, enable);
                    mailHelp.SendHTMLAsync(username, null, new List<string> { assessmentTracking.User.Email }, null, "MunicipalHRPulse Portal assessment review required",
                        rejectedQuestionString, rootDir + @"\wwwroot\EmailTemplates\assessment-rejected.html", loginLink, false);
                    await UpdateQuestionData(assessmentTracking);
                    StateHasChanged();
                }

            }
        }

        public async Task OnYearSelectionChanged(ChangeEventArgs e)
        {
            int selYear;
            SelectedYear = e.Value.ToString();
            if (SelectedYear.Contains("(current)"))
            {
                var tmpString = SelectedYear.Replace("(current)", "");
                selYear = Convert.ToInt32(tmpString);
                ShowingCurrentYear = true;
            }
            else
            {
                selYear = Convert.ToInt32(SelectedYear);
                ShowingCurrentYear = false;
            }

            var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(Municipality, selYear);
            if (assessmentTracking != null)
            {
                await UpdateQuestionData(assessmentTracking);
                StateHasChanged();
            }

        }
    }
}
