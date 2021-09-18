using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityPortal.ViewModels
{
    public class QuestionnaireCategoryViewModel
    {
        public int pkID { get; set; }

        public int SectionNo { get; set; }

        public String Title { get; set; }

        public int Percent { get; set; }

        public bool IsRejected { get; set; }

        public String IconPath { get; set; }

        public bool IsComplete
        {
            get
            {
                return Percent == 100;
            }

        }

        public static explicit operator QuestionnaireCategoryViewModel(QuestionnaireCategory QuestionnaireCategory)
        {
            var viewModel = new QuestionnaireCategoryViewModel()
            {
                pkID= QuestionnaireCategory.pkID,
                SectionNo= QuestionnaireCategory.PageNo,
                Title= QuestionnaireCategory.Title,
                IconPath =QuestionnaireCategory.IconPath
            };

            return viewModel;
        }
    }


}
