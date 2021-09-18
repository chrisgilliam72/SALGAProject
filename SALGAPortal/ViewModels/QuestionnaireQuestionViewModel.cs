using Microsoft.AspNetCore.Http;
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{ 
    public class QuestionnaireQuestionViewModel 
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionNo { get; set; }
        public int PageNo { get; set; }
        public String Question { get; set; }
        public String AnswerType { get; set; }

        public String Comment { get; set; }

        public String SALGAComment { get; set; }

        public String FilePath { get; set; }

        public IFormFile EvidenceFile { get; set; }

        public String FileName { get; set; }
        
        public bool HasCustomResponses { get; set; }

        public List<CustomReponseType> CustomReponses { get; set; }

        public QuestionnaireQuestionViewModel()
        {

        }

        public QuestionnaireQuestionViewModel(QuestionnaireQuestion questionnaireQuestion)
        {
            QuestionID = questionnaireQuestion.pkID;
            PageNo = questionnaireQuestion.PageNo;
            Question = questionnaireQuestion.Question;
            QuestionNo = questionnaireQuestion.QuestionNo;
            HasCustomResponses = questionnaireQuestion.HasCustomResponsesTypes;
        }
    }
}
