
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{
    public class MaturityLevelDashboardViewModel
    {
        public Municipality Municipality { get; set; }
        public List<MaturityLevelSectionViewModel> Sections { get; set; }

        public List<IGrouping<int,MaturityLevelSectionViewModel>> GroupedLevelSections
        {
            get
            {
                return Sections.GroupBy(x => x.QuestionCategoryLevels.MaturityLevel).ToList();
            }
        }

        public OverallMaturityLevels OverallMaturityLevels { get; set; }
        public MaturityLevelSectionsMatrix SectionsMatrix { get; set; }

        public OverallMaturityLevelScorecardViewModel OverallMaturityLevelScorecard { get; set; }
        public MaturityLevelDashboardViewModel()
        {
            Sections = new List<MaturityLevelSectionViewModel>();
            OverallMaturityLevels = new OverallMaturityLevels();
            SectionsMatrix = new MaturityLevelSectionsMatrix();
            OverallMaturityLevelScorecard = new OverallMaturityLevelScorecardViewModel();
        }
    }
}
