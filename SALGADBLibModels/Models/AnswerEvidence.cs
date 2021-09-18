using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class AnswerEvidence
    {
        public int pkID { get; set; }
        public int QuestionNo { get; set; }
        public String OriginalFileName { get; set; }
        public String FileUrl { get; set; }
        public String BlobName { get; set; }
        public String ContainerName { get; set; }
        public DateTime DateStamp { get; set; }
        public Municipality Municipality { get; set; }
    }
}
