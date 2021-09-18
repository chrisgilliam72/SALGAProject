using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGAPortal.ViewModels;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class ValueChainBreakdown
    {

        public List<SALGADashboardCategoryScorecardViewModel> Sections { get; set; }

        public ValueChainBreakdown()
        {
            Sections = new List<SALGADashboardCategoryScorecardViewModel>();
        }
        public void Update(MaturityLevelDashboardViewModel maturityLevelDashboardViewModel)
        {
            Sections.Clear();
            if (maturityLevelDashboardViewModel!=null)
            {
              
                var sections = maturityLevelDashboardViewModel.Sections;
                foreach (var section in sections)
                {
                    var viewModelSection = new SALGADashboardCategoryScorecardViewModel();
                    viewModelSection.Name = section.CategoryName;
                    viewModelSection.IconPath = section.IconPath;
                    viewModelSection.QuestionsPerLevel.Level1 = Convert.ToInt32(section.QuestionCategoryLevels.NoQuestionsLevel1);
                    viewModelSection.QuestionsPerLevel.Level2 = Convert.ToInt32(section.QuestionCategoryLevels.NoQuestionsLevel2);
                    viewModelSection.QuestionsPerLevel.Level3 = Convert.ToInt32(section.QuestionCategoryLevels.NoQuestionsLevel3);
                    viewModelSection.QuestionsPerLevel.Level4 = Convert.ToInt32(section.QuestionCategoryLevels.NoQuestionsLevel4);
                    viewModelSection.ScorePerLevel.Level1 = section.QuestionCategoryLevels.Level1;
                    viewModelSection.ScorePerLevel.Level2 = section.QuestionCategoryLevels.Level2;
                    viewModelSection.ScorePerLevel.Level3 = section.QuestionCategoryLevels.Level3;
                    viewModelSection.ScorePerLevel.Level4 = section.QuestionCategoryLevels.Level4;
                    viewModelSection.PercentageScore.Level1 = section.QuestionCategoryLevels.Level1Percentage;
                    viewModelSection.PercentageScore.Level2 = section.QuestionCategoryLevels.Level2Percentage;
                    viewModelSection.PercentageScore.Level3 = section.QuestionCategoryLevels.Level3Percentage;
                    viewModelSection.PercentageScore.Level4 = section.QuestionCategoryLevels.Level4Percentage;
                    viewModelSection.FunctionalLevel = section.MaturityLevel;
                    Sections.Add(viewModelSection);
                }

   
            }
            StateHasChanged();
        }

    }
}
