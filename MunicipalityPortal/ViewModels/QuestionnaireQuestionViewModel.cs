using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAWeb.ViewModels
{ 
    public class QuestionnaireQuestionViewModel 
    {

        public int QuestionID { get; set; }
        public int QuestionNo { get; set; }
        public int EvidenceID { get; set; }
        public int PageNo { get; set; }
        public int AnswerID { get; set; }
        public String Question { get; set; }
        public String AnswerType { get; set; }

        public String Comment { get; set; }

        public String ApproverComment { get; set; }

        public String FileUrl { get; set; }

        public IFormFile EvidenceFile { get; set; }

        public String FileName { get; set; }
        public String FileBlobName { get; set; }
        public DateTime FileDateStamp { get; set; }
        public bool IsRejected { get; set; }

        public bool HasCustomResponses { get; set; }

        public bool AllowPartial { get; set; }
        public String CustomResponse { get; set; }

        public bool DeleteEvidenceFile { get; set; }

        public List<SelectListItem> ListCustomResponseTypes { get; set; }
        public QuestionnaireQuestionViewModel()
        {
            HasCustomResponses = false;
            ListCustomResponseTypes = new List<SelectListItem>();
        }

        public QuestionnaireQuestionViewModel(QuestionnaireQuestion questionnaireQuestion)
        {
            QuestionID = questionnaireQuestion.pkID;
            PageNo = questionnaireQuestion.PageNo;
            Question = questionnaireQuestion.Question;
            QuestionNo = questionnaireQuestion.QuestionNo;
            HasCustomResponses = questionnaireQuestion.HasCustomResponsesTypes;
            AllowPartial = questionnaireQuestion.AllowPartial;

        }
    }
}
