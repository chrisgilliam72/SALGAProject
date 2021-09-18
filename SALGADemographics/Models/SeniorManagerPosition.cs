using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class SeniorManagerPosition
    {
        public int pkID { get; set; }
        public String Name { get; set; }
        public String PortfolioDisplayValue { get; set; }
        public DateTime AppointmentDate { get; set; }
        public MunicipalityDemographics MunicipalityDemographics { get; set; }
        public SeniorManager SeniorManager { get; set; }
    }
}
