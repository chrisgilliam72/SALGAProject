using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGAPortal.ViewModels;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AssessmentManagement
    {
        public CompletenessReportViewModel ReportData { get; set; }

        [Inject]
        private IAssessmentRepository assessmentRepository { get; set; }

        [Inject]
        public IDemographicsRepository demographicsRepository { get; set; }

        public ProvinceCompleteRow SelectedProvinceData { get; set; }

        public ProvincialAssessmentCompletion ProvincialAssessmentCompletion { get; set; }

        public AssessmentManagement()
        {
     
        }

        protected override async Task OnInitializedAsync()
        {
            var currentYear = DateTime.Today.Year;
            ReportData = await AssessmentManagementReport.GetCompletenessReport(assessmentRepository, demographicsRepository, currentYear);
       
        }

        public void OnUpdateProvinceCompletionData(ProvinceCompleteRow provinceCompleteRow)
        {
            ProvincialAssessmentCompletion.Update(provinceCompleteRow);
        }
    }
}
