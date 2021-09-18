using Microsoft.AspNetCore.Components;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AssessmentsCommunication
    {
       public  OpenAssessmentsViewModel OpenAssessmentsViewModel { get; set; }

        public AssessmentsCommunication()
        {
            OpenAssessmentsViewModel = new OpenAssessmentsViewModel();
        }



        public void SetMunicipalityList(List<AssessmentMunicipalityInfo> municipalityNames)
        {
            OpenAssessmentsViewModel.ToMunicipalities = municipalityNames;
            StateHasChanged();
        }

        public void UpdateTextFromDateChange()
        {
            OpenAssessmentsViewModel.RefreshPredefinedSubjectAndBody();
            StateHasChanged();
        }

    }
}
