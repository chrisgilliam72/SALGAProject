using System;
using System.Collections.Generic;
using System.Text;

namespace SALGASharedReporting.ViewModels
{
    public class OverallMaturityLevelScorecardRow
    {
        public String SectionName { get; set; }
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int Level4 { get; set; }
    }

    public class OverallMaturityLevelScorecardViewModel
    {

        public List<OverallMaturityLevelScorecardRow> Rows { get; set; }

        public void AddRow(String sectionName, int level1, int level2, int level3, int level4)
        {
            var row = new OverallMaturityLevelScorecardRow { SectionName = sectionName, Level1 = level1, Level2 = level2, Level3 = level3, Level4 = level4 };
            Rows.Add(row);
        }

        public OverallMaturityLevelScorecardViewModel()
        {
            Rows = new List<OverallMaturityLevelScorecardRow>();
        }
    }
}
