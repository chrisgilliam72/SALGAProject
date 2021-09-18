using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{

    public class QuestionnaireQuestionViewModel
    {
        public int QuestionNo { get; set; }
        public int PageNo { get; set; }
        public String Question { get; set; }
        public String Note { get; set; }
        public QuestionnaireResponseType AnswerType { get; set; }
        public String Comments { get; set; }

        public String AnswerTypeDisplayString
        {
            get
            {              
                return (AnswerType != null && AnswerType.ResponseType != null) ? AnswerType.ResponseType.ToLower():"";
            }
        }
    }
}
