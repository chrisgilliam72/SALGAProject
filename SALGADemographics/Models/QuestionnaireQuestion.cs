using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class QuestionnaireQuestion
    {
        public int pkID { get; set; }
        public int PageNo { get; set; }
        public int QuestionNo { get; set; }
        public String Question { get; set; }
        public bool Level1 { get; set; }
        public bool Level2 { get; set; }
        public bool Level3 { get; set; }
        public bool Level4 { get; set; }

        public bool Level1Partial { get; set; }
        public bool Level2Partial { get; set; }
        public bool Level3Partial { get; set; }
        public bool Level4Partial { get; set; }
        public bool AllowPartial { get; set; }
        public bool HasCustomResponsesTypes { get; set; }

        public QuestionSet QuestionSet { get; set; }
    }
}
