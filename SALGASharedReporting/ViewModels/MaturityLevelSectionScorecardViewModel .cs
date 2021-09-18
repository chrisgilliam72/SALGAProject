using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SALGASharedReporting.ViewModels
{
    public class MaturityLevelSectionScorecardRow
    {
        public int QuestionNo { get; set; }
        public char[] Scores { get; set; }

        public MaturityLevelSectionScorecardRow()
        {
            Scores = new char[4] { ' ', ' ', ' ', ' ' };
        }
    }


    public class MaturityLevelSectionScorecardViewModel
    {

        public int FunctionalLevel { get; set; }
        public int PageNo { get; set; }
        public String SectionName { get; set; }
        public List<MaturityLevelSectionScorecardRow> Rows { get; set; }

        public MaturityLevelSectionScorecardViewModel()
        {
            Rows = new List<MaturityLevelSectionScorecardRow>();
        }

        public double NoQuestionsQ1 { get; set; }
        public double NoQuestionsQ2 { get; set; }
        public double NoQuestionsQ3 { get; set; }
        public double NoQuestionsQ4 { get; set; }

        public int Score(int level)
        {
            int noQuestions = Rows.Where(x => x.Scores[level - 1] == '1').Count();
            return noQuestions;
        }

        public int PercentScore(int level)
        { 

            double noQUestions = 1;
            switch (level)
            {
                case 1: noQUestions = NoQuestionsQ1;break;
                case 2: noQUestions = NoQuestionsQ2; break;
                case 3: noQUestions = NoQuestionsQ3; break;
                case 4: noQUestions = NoQuestionsQ4; break;
            }

            if (noQUestions>0)
            {
                double score = Score(level);
                double percent = (score / noQUestions) * 100;
                return Convert.ToInt32(percent);
            }

            return 0;
        }

        public void Build(IEnumerable<QuestionnaireQuestionAnswer> questionAnswers, IEnumerable<CustomReponseType> customReponseTypes)
        {

            var questions = questionAnswers.Where(x => x.Question.PageNo == PageNo).OrderBy(x => x.Question.QuestionNo).ToList();
            foreach (var question in questions)
            {
                var row = new MaturityLevelSectionScorecardRow();
                row.QuestionNo = question.Question.QuestionNo;
                if (question.Question.HasCustomResponsesTypes)
                {
                    var scoreResponse = customReponseTypes.FirstOrDefault(x => x.CustomResponse == question.CustomResponse
                                                                          && x.QuestionNo == question.Question.QuestionNo);
                    if (scoreResponse!=null)
                    {
                        if (scoreResponse.Level1)
                            row.Scores[0] = '1';
                        if (scoreResponse.Level2)
                            row.Scores[1] = '1';
                        if (scoreResponse.Level3)
                            row.Scores[2] = '1';
                        if (scoreResponse.Level4)
                            row.Scores[3] = '1';
  
                    }

                }
                else
                {
                    if (question.ResponseType != null)
                    {
                        var response = question.ResponseType.ResponseType.ToLower();
                        if (question.Question.Level1)
                            UpdateRowScore(row, response, 1, true);
                        if (question.Question.Level2)
                        {
                            if (response != "partially")
                                UpdateRowScore(row, response, 2, false);
                            else
                            {
                                if (question.Question.Level2Partial)
                                    UpdateRowScore(row, response, 2, true);
                                else
                                    UpdateRowScore(row, response, 2, false);
                            }
                        }
                        if (question.Question.Level3)
                        {
                            if (response != "partially")
                                UpdateRowScore(row, response, 3, false);
                            else
                            {
                                if (question.Question.Level3Partial)
                                    UpdateRowScore(row, response, 3, true);
                                else
                                    UpdateRowScore(row, response, 3, false);
                            }
                        }

                        if (question.Question.Level4)
                            UpdateRowScore(row, response, 4, false);
                    }
                }

                Rows.Add(row);
            }
        }

        private void UpdateRowScore(MaturityLevelSectionScorecardRow row, String response, int level, bool partialAsYes)
        {

            switch (response)
            {
                case "yes": row.Scores[level-1] = '1'; break;
                case "partially": if (partialAsYes) row.Scores[level - 1] = '1'; else row.Scores[level - 1] = '0';  break;
                case "no": row.Scores[level-1] = '0'; break;

            }
        }
        
    }
}
