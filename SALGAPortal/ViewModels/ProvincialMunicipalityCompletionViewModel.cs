using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public enum MunicipalityCompletionStatus { Complete, Incomplete, NotStarted }
    public class ProvincialMunicipalityCompletionViewModel
    {
        public int ID { get; set; }
        public String Name { get; set; }

        public bool Selected { get; set; }

        public MunicipalityCompletionStatus Status { get; set; }

    }
}
