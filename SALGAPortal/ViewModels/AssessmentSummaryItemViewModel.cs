using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal
{
    public class AssessmentSummaryItemViewModel
    {
        public String MuncipalityName { get; set; }
        public String Type { get; set; }
        public String Province { get; set; }
        public int OverallMaturityLevel { get; set; }

        public Dictionary<String, int> MaturityLevelValues { get; set; }

        public AssessmentSummaryItemViewModel()
        {
            MaturityLevelValues = new Dictionary<string, int>();
        }
    }
}
