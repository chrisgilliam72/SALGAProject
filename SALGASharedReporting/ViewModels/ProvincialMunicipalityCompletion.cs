using System;
using System.Collections.Generic;
using System.Text;

namespace SALGASharedReporting.ViewModels
{
    public enum MunicipalityCompletionStatus { Complete, Incomplete, NotStarted }
    public class ProvincialMunicipalityCompletion
    {
        public int ID { get; set; }
        public String Name { get; set; }

        public bool Selected { get; set; }

        public MunicipalityCompletionStatus Status { get; set; }

    }
}
