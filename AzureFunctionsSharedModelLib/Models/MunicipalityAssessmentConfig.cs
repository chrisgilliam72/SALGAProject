using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsSharedModelLib
{
    public class MunicipalityAssessmentConfig
    {
        public int pkID { get; set; }
        public Municipality Municipality { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentQuestionSet { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }

    }
}
