using SALGADBLib;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting
{
    public class MunicipalityDashboardReport
    {
        private List<AssessmentTracking> AssessmentTrackings { get; set; }
        private List<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        private List<QuestionnaireCategory> QuestionnairePages { get; set; }
        private List<QuestionAnswerNotes> QuestionAnswerNotes { get; set; }
        private List<CustomReponseType> CustomReponseTypes { get; set; }
        private List<QuestionnaireResponseType> QuestionnaireResponseTypes { get; set; }

        public static async Task<MaturityLevelDashboardViewModel> LoadTemplate(IAssessmentRepository assessmentRepository)
        {
            var MaturityLevelDashboardViewModel = new MaturityLevelDashboardViewModel();
            var questionnairePages = (await assessmentRepository.GetPageInfo()).ToList();
            var questionnaireQuestions = (await assessmentRepository.GetQuestions()).ToList();
            MaturityLevelDashboardViewModel.Sections = questionnairePages.Select(x => new MaturityLevelSectionViewModel { PageNo = x.PageNo, CategoryName = x.Title, IconPath = x.IconPath }).ToList();
            foreach (var categorySection in MaturityLevelDashboardViewModel.Sections)
            {
                var questions = questionnaireQuestions.Where(x => x.PageNo == categorySection.PageNo);
                categorySection.ScoreCard.SectionName = categorySection.CategoryName;
                categorySection.ScoreCard.PageNo = categorySection.PageNo;
                categorySection.ScoreCard.FunctionalLevel = categorySection.MaturityLevel;
                categorySection.ScoreCard.NoQuestionsQ1 = categorySection.QuestionCategoryLevels.NoQuestionsLevel1;
                categorySection.ScoreCard.NoQuestionsQ2 = categorySection.QuestionCategoryLevels.NoQuestionsLevel2;
                categorySection.ScoreCard.NoQuestionsQ3 = categorySection.QuestionCategoryLevels.NoQuestionsLevel3;
                categorySection.ScoreCard.NoQuestionsQ4 = categorySection.QuestionCategoryLevels.NoQuestionsLevel4;

                foreach (var question in questions)
                {
                    var item = new QuestionnaireQuestionViewModel();
                    item.Question = question.Question;
                    item.QuestionNo = question.QuestionNo;

                    categorySection.QuestionAnswers.Add(item);
                }
            }
            return MaturityLevelDashboardViewModel;
        }
        public static async Task<MaturityLevelDashboardViewModel> LoadMunicipalityReport(Municipality municipality, int assessmentYear, IAssessmentRepository assessmentRepository)
        {
            var MaturityLevelDashboardViewModel = new MaturityLevelDashboardViewModel();
            if (municipality != null)
            {
                MaturityLevelDashboardViewModel.Municipality = municipality;
                var assessmentTracking = await assessmentRepository.GetAssessmentTracking(municipality, assessmentYear);
                var questionnaireNotes = (await assessmentRepository.GetQuestionAnswerNotes()).ToList();
                var questionnairePages = (await assessmentRepository.GetPageInfo()).ToList();
                var questionnaireQuestions = (await assessmentRepository.GetQuestions()).ToList();
                var questionnaireAnswers = (await assessmentRepository.GetQuestionnaireQuestionAnswers(assessmentTracking)).ToList();
                var customReponseTypes = (await assessmentRepository.GetCustomResponseTypes()).ToList();
                var responseTypes = (await assessmentRepository.GetResponseTypes()).ToList();
                var responseYes = responseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "yes");
                var responseNo = responseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "no");
                var responsePartial = responseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "partially");
                var responseCustom = responseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "custom");

                MaturityLevelDashboardViewModel.Sections = questionnairePages.Select(x => new MaturityLevelSectionViewModel { PageNo = x.PageNo, CategoryName = x.Title, IconPath = x.IconPath }).ToList();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.NoCategories = MaturityLevelDashboardViewModel.Sections.Count;
                foreach (var categorySection in MaturityLevelDashboardViewModel.Sections)
                {
                    var answers = questionnaireAnswers.Where(x => x.Question.PageNo == categorySection.PageNo).OrderBy(x => x.Question.QuestionNo).ToList();
                    var questions = questionnaireQuestions.Where(x => x.PageNo == categorySection.PageNo);

                    categorySection.QuestionCategoryLevels.NoQuestionsLevel1 = questionnaireAnswers.Where(x => x.Question.Level1 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel2 = questionnaireAnswers.Where(x => x.Question.Level2 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel3 = questionnaireAnswers.Where(x => x.Question.Level3 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel4 = questionnaireAnswers.Where(x => x.Question.Level4 == true && x.Question.PageNo == categorySection.PageNo).Count();


                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1 = questionnaireAnswers.Where(x => x.Question.Level1 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1 += questionnaireAnswers.Where(x => x.Question.Level1Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2 = questionnaireAnswers.Where(x => x.Question.Level2 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2 += questionnaireAnswers.Where(x => x.Question.Level2Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3 = questionnaireAnswers.Where(x => x.Question.Level3 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3 += questionnaireAnswers.Where(x => x.Question.Level3Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel4 = questionnaireAnswers.Where(x => x.Question.Level4 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();

                    var customAnswers = questionnaireAnswers.Where(x => x.Question.PageNo == categorySection.PageNo && x.Question.HasCustomResponsesTypes).ToList();
                    foreach (var customAnswer in customAnswers)
                    {
                        var customResponse = customReponseTypes.FirstOrDefault(x => x.QuestionNo == customAnswer.Question.QuestionNo && x.CustomResponse == customAnswer.CustomResponse);
                        if (customResponse != null)
                        {
                            if (customResponse.Level1)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1++;
                            if (customResponse.Level2)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2++;
                            if (customResponse.Level3)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3++;
                            if (customResponse.Level4)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel4++;
                        }
                    }


                    categorySection.MunicipalityName = municipality.Name;

                    foreach (var answer in answers)
                    {
                        var item = new QuestionnaireQuestionViewModel();
                        item.AnswerType = answer.ResponseType;
                        item.QuestionNo = answer.Question.QuestionNo;
                        item.Comments = answer.Comments;
                        var note = questionnaireNotes.FirstOrDefault(x => x.QuestionNo == item.QuestionNo);
                        if (note != null)
                        {
                            if (item.AnswerType != null)
                            {
                                switch (item.AnswerType.ResponseType.ToLower())
                                {
                                    case "yes": item.Note = note.Yes; break;
                                    case "no": item.Note = note.No; break;
                                    case "partially": item.Note = note.Partial; break;
                                    default: item.Note = "--------Invalid---------"; break;
                                }
                            }
                            else
                                item.Note = "--------Invalid---------";
                        }
                        else
                        {
                            item.Note = "---------Missing---------";
                        }
                        categorySection.QuestionAnswers.Add(item);
                    }


                    categorySection.ScoreCard.SectionName = categorySection.CategoryName;
                    categorySection.ScoreCard.PageNo = categorySection.PageNo;
                    categorySection.ScoreCard.FunctionalLevel = categorySection.MaturityLevel;
                    categorySection.ScoreCard.NoQuestionsQ1 = categorySection.QuestionCategoryLevels.NoQuestionsLevel1;
                    categorySection.ScoreCard.NoQuestionsQ2 = categorySection.QuestionCategoryLevels.NoQuestionsLevel2;
                    categorySection.ScoreCard.NoQuestionsQ3 = categorySection.QuestionCategoryLevels.NoQuestionsLevel3;
                    categorySection.ScoreCard.NoQuestionsQ4 = categorySection.QuestionCategoryLevels.NoQuestionsLevel4;
                    categorySection.ScoreCard.Build(questionnaireAnswers, customReponseTypes);


                    MaturityLevelDashboardViewModel.OverallMaturityLevelScorecard.AddRow(categorySection.CategoryName,
                                                                                        categorySection.QuestionCategoryLevels.Level1,
                                                                                        categorySection.QuestionCategoryLevels.Level2,
                                                                                        categorySection.QuestionCategoryLevels.Level3,
                                                                                        categorySection.QuestionCategoryLevels.Level4);
                }

                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level1Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level1 >= 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level2Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level2 == 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level3Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level3 == 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level4Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level4 == 1).Count();


            }

            return MaturityLevelDashboardViewModel;
        }


        public async Task<IEnumerable<MaturityLevelDashboardViewModel>> GetAllMaturityLevelReports(IAssessmentRepository assessmentRepository, IDemographicsRepository demographicsRepository)
        {

            List<MaturityLevelDashboardViewModel> listModels = new List<MaturityLevelDashboardViewModel>();
            QuestionAnswerNotes = (await assessmentRepository.GetQuestionAnswerNotes()).ToList();
            QuestionnairePages = (await assessmentRepository.GetPageInfo()).ToList();
            QuestionnaireQuestions = (await assessmentRepository.GetQuestions()).ToList();
            CustomReponseTypes = (await assessmentRepository.GetCustomResponseTypes()).ToList();
            QuestionnaireResponseTypes = (await assessmentRepository.GetResponseTypes()).ToList();

            var municipalities = await demographicsRepository.GetMunicipalities();
            var grpedMunicipalities = municipalities.GroupBy(x => x.Province);
            var trackings = await assessmentRepository.GetAssessmentTrackings();
            var grpedTrackings = trackings.GroupBy(x => x.Municipality).ToList();
            var latestTrackings = new List<AssessmentTracking>();
            foreach (var tracking in grpedTrackings)
            {
                var latestTracking = tracking.OrderByDescending(x => x.AuditYear).First();
                latestTrackings.Add(latestTracking);
            }

            var allAnswers = await assessmentRepository.GetQuestionnaireQuestionAnswers(latestTrackings);
            foreach (var tracking in latestTrackings)
            {
                var answers = allAnswers.Where(x => x.Tracking.pkID == tracking.pkID);
                var viewModel = GetDashboardModel(tracking.Municipality, answers);
                viewModel.Municipality = tracking.Municipality;
                listModels.Add(viewModel);
            }
  
            return listModels.OrderBy(x=>x.Municipality.Name);
        }

        private MaturityLevelDashboardViewModel GetDashboardModel(Municipality municipality, IEnumerable<QuestionnaireQuestionAnswer> questionnaireAnswers)
        {

            var responseYes = QuestionnaireResponseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "yes");
            var responseNo = QuestionnaireResponseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "no");
            var responsePartial = QuestionnaireResponseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "partially");
            var responseCustom = QuestionnaireResponseTypes.FirstOrDefault(x => x.ResponseType.ToLower() == "custom");

            var MaturityLevelDashboardViewModel = new MaturityLevelDashboardViewModel();
            if (municipality != null)
            {


                MaturityLevelDashboardViewModel.Sections = QuestionnairePages.Select(x => new MaturityLevelSectionViewModel { PageNo = x.PageNo, CategoryName = x.Title }).ToList();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.NoCategories = MaturityLevelDashboardViewModel.Sections.Count;
                foreach (var categorySection in MaturityLevelDashboardViewModel.Sections)
                {
                    var answers = questionnaireAnswers.Where(x => x.Question.PageNo == categorySection.PageNo).OrderBy(x => x.Question.QuestionNo).ToList();
                    var questions = QuestionnaireQuestions.Where(x => x.PageNo == categorySection.PageNo);

                    categorySection.QuestionCategoryLevels.NoQuestionsLevel1 = questionnaireAnswers.Where(x => x.Question.Level1 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel2 = questionnaireAnswers.Where(x => x.Question.Level2 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel3 = questionnaireAnswers.Where(x => x.Question.Level3 == true && x.Question.PageNo == categorySection.PageNo).Count();
                    categorySection.QuestionCategoryLevels.NoQuestionsLevel4 = questionnaireAnswers.Where(x => x.Question.Level4 == true && x.Question.PageNo == categorySection.PageNo).Count();


                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1 = questionnaireAnswers.Where(x => x.Question.Level1 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1 += questionnaireAnswers.Where(x => x.Question.Level1Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2 = questionnaireAnswers.Where(x => x.Question.Level2 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2 += questionnaireAnswers.Where(x => x.Question.Level2Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3 = questionnaireAnswers.Where(x => x.Question.Level3 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();
                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3 += questionnaireAnswers.Where(x => x.Question.Level3Partial == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responsePartial.ResponseType).Count();

                    categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel4 = questionnaireAnswers.Where(x => x.Question.Level4 == true && x.Question.PageNo == categorySection.PageNo && x.ResponseType != null).Where(x => x.ResponseType.ResponseType == responseYes.ResponseType).Count();

                    var customAnswers = questionnaireAnswers.Where(x => x.Question.PageNo == categorySection.PageNo && x.Question.HasCustomResponsesTypes).ToList();
                    foreach (var customAnswer in customAnswers)
                    {
                        var customResponse = CustomReponseTypes.FirstOrDefault(x => x.QuestionNo == customAnswer.Question.QuestionNo && x.CustomResponse == customAnswer.CustomResponse);
                        if (customResponse != null)
                        {
                            if (customResponse.Level1)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel1++;
                            if (customResponse.Level2)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel2++;
                            if (customResponse.Level3)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel3++;
                            if (customResponse.Level4)
                                categorySection.QuestionCategoryLevels.QuestionsAnsweredLevel4++;
                        }
                    }


                    categorySection.MunicipalityName = municipality.Name;

                    foreach (var answer in answers)
                    {
                        var item = new QuestionnaireQuestionViewModel();
                        item.AnswerType = answer.ResponseType;
                        item.QuestionNo = answer.Question.QuestionNo;
                        item.Comments = answer.Comments;
                        var note = QuestionAnswerNotes.FirstOrDefault(x => x.QuestionNo == item.QuestionNo);
                        if (note != null)
                        {
                            if (item.AnswerType != null)
                            {
                                switch (item.AnswerType.ResponseType.ToLower())
                                {
                                    case "yes": item.Note = note.Yes; break;
                                    case "no": item.Note = note.No; break;
                                    case "partially": item.Note = note.Partial; break;
                                    default: item.Note = "--------Invalid---------"; break;
                                }
                            }
                            else
                                item.Note = "--------Invalid---------";
                        }
                        else
                        {
                            item.Note = "---------Missing---------";
                        }
                        categorySection.QuestionAnswers.Add(item);
                    }


                    categorySection.ScoreCard.SectionName = categorySection.CategoryName;
                    categorySection.ScoreCard.PageNo = categorySection.PageNo;
                    categorySection.ScoreCard.FunctionalLevel = categorySection.MaturityLevel;
                    categorySection.ScoreCard.NoQuestionsQ1 = categorySection.QuestionCategoryLevels.NoQuestionsLevel1;
                    categorySection.ScoreCard.NoQuestionsQ2 = categorySection.QuestionCategoryLevels.NoQuestionsLevel2;
                    categorySection.ScoreCard.NoQuestionsQ3 = categorySection.QuestionCategoryLevels.NoQuestionsLevel3;
                    categorySection.ScoreCard.NoQuestionsQ4 = categorySection.QuestionCategoryLevels.NoQuestionsLevel4;
                    categorySection.ScoreCard.Build(questionnaireAnswers, CustomReponseTypes);


                    MaturityLevelDashboardViewModel.OverallMaturityLevelScorecard.AddRow(categorySection.CategoryName,
                                                                                        categorySection.QuestionCategoryLevels.Level1,
                                                                                        categorySection.QuestionCategoryLevels.Level2,
                                                                                        categorySection.QuestionCategoryLevels.Level3,
                                                                                        categorySection.QuestionCategoryLevels.Level4);
                }

                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level1Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level1 >= 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level2Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level2 == 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level3Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level3 == 1).Count();
                MaturityLevelDashboardViewModel.OverallMaturityLevels.Level4Points = MaturityLevelDashboardViewModel.Sections.Where(x => x.QuestionCategoryLevels.Level4 == 1).Count();


            }

            return MaturityLevelDashboardViewModel;
        }
    }
}
