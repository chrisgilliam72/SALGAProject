using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class AssessmentMunicipalityInfo
    {
        public int ID { get; set; }
        public String Name { get; set; }
    }

    public class OpenAssessmentsViewModel
    {
        public bool NewAssessmeent { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public String SubjectText { get; set; }

        public String BodyText { get; set; }


        public List<AssessmentMunicipalityInfo> ToMunicipalities { get; set; }

        private int GetMonthDifference()
        {
            int monthsApart = 12 * (StartDate.Year - EndDate.Year) + StartDate.Month - EndDate.Month;
            return Math.Abs(monthsApart);
        }


        public OpenAssessmentsViewModel()
        {
            ToMunicipalities = new List<AssessmentMunicipalityInfo>();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddMonths(6);
            RefreshPredefinedSubjectAndBody();
        }

        public void RefreshPredefinedSubjectAndBody()
        {
            SubjectText = StartDate.Year + " self-assessment";
            BodyText = "Good Day," + Environment.NewLine;
            BodyText += "You have been allocated to " + StartDate.Year + " self-assessment to complete." + Environment.NewLine;
            BodyText += "You are allocated " + GetMonthDifference() + " months to complete the assessment. You may access the self-assessment on your Municipal HR Pulse account." + Environment.NewLine;
            BodyText += Environment.NewLine + "Regards," + Environment.NewLine;
            BodyText += "SALGA";
        }
    }
}
