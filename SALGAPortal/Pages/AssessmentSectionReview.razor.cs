using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AssessmentSectionReview
    {
    
        [Parameter]
        public List<QuestionnaireQuestionViewModel> ListQuestionsAnswers { get; set; }
        [Parameter]
        public List<QuestionnaireResponseType> ResponseTypes { get; set; }
        [Parameter]
        public bool ShowSave { get; set; }
        [Parameter]
        public bool ViewOnly { get; set; }
        [Inject]
        IAssessmentRepository _assessmentRepository { get; set; }

  
       
        public async Task OnSave()
        {


            var dbAnswers = new List< QuestionnaireQuestionAnswer>();
            foreach(var answer in ListQuestionsAnswers)
            {
                var dbQuestionnaireAnswer = new QuestionnaireQuestionAnswer()
                {
                    pkID = answer.AnswerID,
                    SALGAComments=answer.SALGAComment,
                    ResponseType = ResponseTypes.FirstOrDefault(x=>x.ResponseType==answer.AnswerType)
                };

                dbAnswers.Add(dbQuestionnaireAnswer);
            }

            await _assessmentRepository.SaveReviewedAnswers(dbAnswers);
        }
    }
}
