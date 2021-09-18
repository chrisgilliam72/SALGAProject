using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class CustomReponseType
    {
        public int pkID { get; set; }
        public int QuestionNo { get; set; }
        public int SectionNo { get; set; }
        public String CustomResponse { get; set; }

        public bool Level1 { get; set; }
        public bool Level2 { get; set; }
        public bool Level3 { get; set; }
        public bool Level4 { get; set; }

        public bool VisibleToUser { get; set; }
        public bool VisibleToApprover { get; set; }
    }
}
