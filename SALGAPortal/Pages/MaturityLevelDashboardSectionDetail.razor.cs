using Microsoft.AspNetCore.Components;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class MaturityLevelDashboardSectionDetail
    {
        [Parameter]
        public MaturityLevelSectionViewModel MaturityLevelSectionViewModel { get; set; }
    }
}
