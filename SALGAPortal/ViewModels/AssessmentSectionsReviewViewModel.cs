using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class AssessmentSectionsReviewViewModel
    {
        public List<QuestionnaireQuestionViewModel> ListQuestionsAnswers { get; set; }

        public AssessmentSectionsReviewViewModel()
        {
            ListQuestionsAnswers = new List<QuestionnaireQuestionViewModel>();
        }
    }
}
