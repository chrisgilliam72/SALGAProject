using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class MaturityLevelSummary
    {
        private MaturityLevelDashboardViewModel MaturityLevelDashboardViewModel { get; set; }

        public void Update(MaturityLevelDashboardViewModel maturityLevelDashboardViewModel)
        {
            MaturityLevelDashboardViewModel = maturityLevelDashboardViewModel;
            StateHasChanged();
        }
    }
}
