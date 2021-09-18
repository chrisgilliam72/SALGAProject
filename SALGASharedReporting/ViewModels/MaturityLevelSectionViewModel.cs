using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{
    public class MaturityLevelSectionViewModel
    {
        public int PageNo { get; set; }
        public String CategoryName { get; set; }
        public QuestionCategoryLevels QuestionCategoryLevels { get; set; }
        public List<QuestionnaireQuestionViewModel> QuestionAnswers { get; set; }

        public MaturityLevelSectionScorecardViewModel ScoreCard { get; set; }

        public String IconPath { get; set; }
        public String MunicipalityName { get; set; }

        public int MaturityLevel
        {
            get
            {
                return QuestionCategoryLevels.MaturityLevel;
            }
        }
        public MaturityLevelSectionViewModel()
        {
            QuestionCategoryLevels = new QuestionCategoryLevels();
            QuestionAnswers = new List<QuestionnaireQuestionViewModel>();
            ScoreCard = new MaturityLevelSectionScorecardViewModel();
        }
    }
}
