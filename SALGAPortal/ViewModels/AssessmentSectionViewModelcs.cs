using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class AssessmentSectionViewModelcs
    {
        public String Name { get; set; }
        public int PercentComplete { get; set; }

        public String IconPath { get; set; }

        public bool IsComplete
        {
            get
            {
                return PercentComplete == 100;
            }
        }
    }
}
