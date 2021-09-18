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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using SALGADBLib;
using SALGAEvidenceRepository;
using SALGAWeb.ViewModels;

namespace MunicipalityPortal.Pages
{
    [Authorize]
    public class AssessmentSectionDetailModel : PageModel
    {
        public List<QuestionnaireCategory> ListPages { get; set; }
        public List<SelectListItem> ListResponseTypes { get; set; }
        public List<SelectListItem> ListResponseTypesNoPartial { get; set; }
        [BindProperty(SupportsGet = true)]
        public int SectionNo { get; set; }
        [BindProperty]
        public List<QuestionnaireQuestionViewModel> ListQuestionsAnswers { get; set; }
        [BindProperty]
        public String SectionTitle { get; set; }
        [BindProperty]
        public bool IsApprover { get; set; }

        public bool IsApproved { get; set; }
        public bool IsSubmitted { get; set; }
        public bool CanEdit
        {
            get
            {
                if (!IsApprover)
                    return (!IsApproved && !IsSubmitted);
                else
                {
                    return IsSubmitted && !IsApproved;
                }

            }

        }

        private IAssessmentRepository _assessmentRepository;
        private IDemographicsRepository _demographicsRepository;
        private UserManager<IdentityUser> _userManager;
        private IAzureRepository _evdenceRepository;
        private IConfiguration _configuration;
        private IAuditingRepository _auditingRepository;
        public AssessmentSectionDetailModel(IDemographicsRepository demographicsRepository, IAssessmentRepository assessmentRepository,
                                            UserManager<IdentityUser> userManager, IAzureRepository evidenceRepository, IConfiguration configuration,
                                            IAuditingRepository auditingRepository)
        {
            _demographicsRepository = demographicsRepository;
            _assessmentRepository = assessmentRepository;
            _userManager = userManager;
            _evdenceRepository = evidenceRepository;
            _configuration = configuration;
            IsApprover = false;
            _auditingRepository = auditingRepository;
        }

        public async Task OnGet(int SectionNo)
        {
            try
            {
                List<QuestionnaireQuestionAnswer> lstAnswers = null;
                var user = await _userManager.GetUserAsync(User);
                var demographics = await _demographicsRepository.GetDemographics(user);
                IsApprover = await _userManager.IsInRoleAsync(user, "Approver");
                ListPages = (await _assessmentRepository.GetPageInfo()).ToList();
                var listQuestions = (await _assessmentRepository.GetQuestions()).ToList();
                var questionnaireResponseTypes = (await _assessmentRepository.GetResponseTypes()).ToList();
                var customResponseTypes = (await _assessmentRepository.GetCustomResponseTypes()).ToList();

                ListQuestionsAnswers = listQuestions.Where(x => x.PageNo == SectionNo).Select(x => new QuestionnaireQuestionViewModel(x)).ToList();
                this.SectionNo = ListPages.First(x => x.PageNo == SectionNo).PageNo;
                SectionTitle = ListPages.First(x => x.PageNo == SectionNo).Title;

                if (IsApprover)
                {

                    var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                    var tracking = await _assessmentRepository.GetAssessmentTracking(demographics.Capturer, config.CurrentYear);
                    if (tracking != null && tracking.IsSubmitted && tracking.IsApproved)
                        RedirectToPage("/AssessmentQuestions");

                    ListResponseTypes = questionnaireResponseTypes.Where(x=>x.VisibleToApprover).Select(x => new SelectListItem { Text = x.ResponseType, Value = x.ResponseType }).ToList();
                    lstAnswers = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(tracking)).ToList();

                    var customResponseQues = ListQuestionsAnswers.Where(x => x.HasCustomResponses == true);
                    foreach (var question in customResponseQues)
                    {
                        var lstResponses = customResponseTypes.Where(x => x.QuestionNo == question.QuestionNo && x.VisibleToApprover).Select(x => new SelectListItem(x.CustomResponse, x.CustomResponse)).ToList();
                        question.ListCustomResponseTypes = lstResponses;
                    }

                    IsSubmitted = tracking.IsSubmitted;
                    IsApproved = tracking.IsApproved;
                }
                else
                {
                    var config = await _assessmentRepository.GetAssessmentConfig(demographics.Municipality);
                    var tracking = await _assessmentRepository.GetAssessmentTracking(user, config.CurrentYear);
                    if (tracking != null && tracking.IsSubmitted)
                        RedirectToPage("/AssessmentQuestions");

                    ListResponseTypes = questionnaireResponseTypes.Where(x => x.VisibleToUser).Select(x => new SelectListItem { Text = x.ResponseType, Value = x.ResponseType }).ToList();
                    lstAnswers = (await _assessmentRepository.GetQuestionnaireQuestionAnswers(tracking)).ToList();

                    var customResponseQues = ListQuestionsAnswers.Where(x => x.HasCustomResponses == true);
                    foreach (var question in customResponseQues)
                    {
                        var lstResponses = customResponseTypes.Where(x => x.QuestionNo == question.QuestionNo && x.VisibleToUser).Select(x => new SelectListItem(x.CustomResponse, x.CustomResponse)).ToList();
                        question.ListCustomResponseTypes = lstResponses;
                    }

                    IsSubmitted = tracking.IsSubmitted;
                    IsApproved = tracking.IsApproved;
                }




                foreach (var questonAnswer in ListQuestionsAnswers)
                {
                    var answer = lstAnswers.FirstOrDefault(x => x.Question.QuestionNo == questonAnswer.QuestionNo);
                    if (answer != null)
                    {

                        questonAnswer.Comment = answer.Comments;
                        questonAnswer.FileName = answer.AnswerEvidence != null ? answer.AnswerEvidence.OriginalFileName : "";
                        questonAnswer.FileUrl = answer.AnswerEvidence != null ? answer.AnswerEvidence.FileUrl : "";
                        questonAnswer.FileDateStamp = answer.AnswerEvidence != null ? answer.AnswerEvidence.DateStamp : DateTime.Now;
                        questonAnswer.FileBlobName = answer.AnswerEvidence != null ? answer.AnswerEvidence.BlobName : "";
                        questonAnswer.EvidenceID = answer.AnswerEvidence != null ? answer.AnswerEvidence.pkID : 0;
                        questonAnswer.AnswerType = answer.ResponseType != null ? answer.ResponseType.ResponseType : "";
                        questonAnswer.AnswerID = answer.pkID;
                        questonAnswer.ApproverComment = answer.ApproverComments;
                        questonAnswer.IsRejected = (questonAnswer.AnswerType == "Rejected") || (questonAnswer.HasCustomResponses && answer.CustomResponse=="Rejected");
                        questonAnswer.CustomResponse = answer.CustomResponse;
                    }
                }

                ListResponseTypesNoPartial = ListResponseTypes.ToList();

                var partialItem = ListResponseTypesNoPartial.FirstOrDefault(x => x.Text.ToLower() == "partially");
                if (partialItem != null)
                    ListResponseTypesNoPartial.Remove(partialItem);

            }
            catch (Exception ex)
            {
                RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }

        public ActionResult OnGetDownloadEvidenceFile(String blobName)
        {
            try
            {
                var storageKey = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:EvidenceFiles"];
                var containerClient = new BlobContainerClient(storageKey, containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                MemoryStream stream = new MemoryStream();
                var blobDownloadInfo = blobClient.DownloadTo(stream);
                stream.Position = 0;
                return File(stream, "application/octet-stream", blobName);
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
                List<QuestionnaireQuestionAnswer> dbQuestionAnswers = new List<QuestionnaireQuestionAnswer>();
                var identityUser = await _userManager.GetUserAsync(User);
                var intervieweeDetails = await _demographicsRepository.GetIntervieweeDetails(identityUser);


                if (intervieweeDetails != null)
                {
                    var questions = await _assessmentRepository.GetQuestions();
                    var responseTypes = await _assessmentRepository.GetResponseTypes();
                    var config = await _assessmentRepository.GetAssessmentConfig(intervieweeDetails.Municipality);
                    var user = await _userManager.GetUserAsync(User);
                    var storageKey = _configuration["AzureStorage:ConnectionString"];
                    var containerName = _configuration["AzureStorage:EvidenceFiles"];
                    _evdenceRepository.SetConnectionString(storageKey);
                    foreach (var answer in ListQuestionsAnswers)
                    {


                        var dbQuestionnaireAnswer = new QuestionnaireQuestionAnswer()
                        {
                            Question = questions.FirstOrDefault(x => x.pkID == answer.QuestionID),
                            Comments = answer.Comment,
                            ResponseType = answer.HasCustomResponses ? responseTypes.FirstOrDefault(x => x.ResponseType == "Custom") :
                                                                       responseTypes.FirstOrDefault(x => x.ResponseType == answer.AnswerType),
                            User = identityUser,
                            Municipality = intervieweeDetails.Municipality,
                            pkID = IsApprover ? answer.AnswerID : 0,
                            ApproverComments = answer.ApproverComment,
                            CustomResponse = answer.HasCustomResponses ? answer.CustomResponse : null

                        };
                        if (answer.DeleteEvidenceFile)
                        {
             
                            await _evdenceRepository.DeleteFile(containerName, answer.FileBlobName);
                            await _assessmentRepository.DeleteEvidence(answer.EvidenceID);
                            dbQuestionnaireAnswer.AnswerEvidence = null;
                            
                        }
                        else if (answer.EvidenceFile != null)
                        {

                            var evidence = new AnswerEvidence();
                            evidence.Municipality = intervieweeDetails.Municipality;
                            evidence.OriginalFileName = answer.EvidenceFile.FileName;
                            evidence.ContainerName = containerName;
                            evidence.BlobName = intervieweeDetails.Municipality.Name + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + "_" + answer.EvidenceFile.FileName;
                            evidence.DateStamp = DateTime.Now;
                            evidence.QuestionNo = answer.QuestionNo;
                            using (var memStream = new MemoryStream())
                            {
                                answer.EvidenceFile.CopyTo(memStream);
                                answer.FileUrl = await _evdenceRepository.AddFile(containerName, evidence.BlobName, memStream);
                                answer.FileName = answer.EvidenceFile.FileName;
                            }

                            evidence.FileUrl = answer.FileUrl;
                            dbQuestionnaireAnswer.AnswerEvidence = evidence;

                        }
                    
                        else if (answer.EvidenceID!=0)
                        {
                            var evidence = new AnswerEvidence();
                            evidence.Municipality = intervieweeDetails.Municipality;
                            evidence.OriginalFileName = answer.FileName;
                            evidence.ContainerName = containerName;
                            evidence.BlobName = answer.FileBlobName;
                            evidence.FileUrl = answer.FileUrl;
                            evidence.DateStamp = answer.FileDateStamp;
                            evidence.QuestionNo = answer.QuestionNo;
                            dbQuestionnaireAnswer.AnswerEvidence = evidence;
                        }

                        dbQuestionAnswers.Add(dbQuestionnaireAnswer);
                    }

                    if (dbQuestionAnswers.Count > 0)
                    {
                        if (IsApprover)
                        {
                            
                            await _assessmentRepository.SaveReviewedAnswers(dbQuestionAnswers);
                        }

                        else
                        {
                            var assessmentTracking = await _assessmentRepository.GetAssessmentTracking(user, config.CurrentYear);
                            if (assessmentTracking == null)
                                assessmentTracking = await _assessmentRepository.CreateAssessmentTracking(user, intervieweeDetails.Municipality, config.CurrentYear);
                            await _assessmentRepository.DeleteQuestionannaireAnswers(assessmentTracking, SectionNo);
                            await _assessmentRepository.AddQuestionAnswers(dbQuestionAnswers, assessmentTracking);
                            await _auditingRepository.AddAssessmentModifiedEvent(user, intervieweeDetails.Municipality.Name, SectionTitle);

                        }
                    }

                    var questionnaireResponseTypes = (await _assessmentRepository.GetResponseTypes()).ToList();
                    ListResponseTypes = questionnaireResponseTypes.Select(x => new SelectListItem { Text = x.ResponseType, Value = x.ResponseType }).ToList();
                }



            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }

            return RedirectToPage("/AssessmentQuestions");
        }
    }
}
