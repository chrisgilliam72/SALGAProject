using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class QuestionnaireQuestionAnswer
    {
        public int pkID { get; set; }
        public QuestionnaireQuestion Question { get; set; }
        public QuestionnaireResponseType ResponseType{ get; set; }
        public String Comments { get; set; }
        public String ApproverComments { get; set; }
        public String SALGAComments { get; set; }

        public IdentityUser User { get; set; }
        public Municipality Municipality { get; set; }
        public AssessmentTracking Tracking { get; set; }

        public AnswerEvidence AnswerEvidence { get; set; }

        public String CustomResponse { get; set; }
    }
}
