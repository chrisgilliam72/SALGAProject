using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class QuestionAnswerNotes
    {
        public int pkID { get; set; }
        public int QuestionNo { get; set; }

        public String Yes { get; set; }
        public String No { get; set; }

        public String Partial { get; set; }
        public String Interviewee { get; set; }
    }
}
