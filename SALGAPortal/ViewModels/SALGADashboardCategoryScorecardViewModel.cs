using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class SALGADashboardCategoryScorecardRow
    {
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int Level4 { get; set; }
    }
    public class SALGADashboardCategoryScorecardViewModel
    {
        public String Name { get; set; }
        public int FunctionalLevel { get; set; }

        public String IconPath { get; set; }
        public SALGADashboardCategoryScorecardRow QuestionsPerLevel { get; set; }
        public SALGADashboardCategoryScorecardRow ScorePerLevel { get; set; }
        public SALGADashboardCategoryScorecardRow PercentageScore { get; set; }

        public SALGADashboardCategoryScorecardViewModel()
        {
            QuestionsPerLevel = new SALGADashboardCategoryScorecardRow();
            PercentageScore = new SALGADashboardCategoryScorecardRow();
            ScorePerLevel = new SALGADashboardCategoryScorecardRow();
        }

    }
}
