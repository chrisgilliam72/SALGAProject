using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class MaturityLevelsDashboard 
    {
        [Parameter]
        public Municipality Municipality { get; set; }
        [Inject]
        public IAssessmentRepository AssessmentRepository { get; set; }

        [Inject]
        public IDemographicsRepository DemographicsRepository { get; set; }

        private MaturityLevelDashboardViewModel MaturityLevelDashboardViewModel { get; set; }

        public MaturityLevelsDashboard()
        {
            MaturityLevelDashboardViewModel = new MaturityLevelDashboardViewModel();
        }

      

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
     
            await UpdateCharts();
        }

        public async Task InitCharts()
        {
            if (MaturityLevelDashboardViewModel!=null && MaturityLevelDashboardViewModel.Sections!=null)
            {
                foreach (var categorySection in MaturityLevelDashboardViewModel.Sections)
                {
                    string chartName = "chart" + categorySection.PageNo;
                    var paramArray = new object[]
                    {
                    0, 0,0,0 ,chartName,categorySection.CategoryName

                    };
                    await JS.InvokeAsync<object>("drawChart", paramArray);
                }
            }

        }

        public void SetTemplate(MaturityLevelDashboardViewModel maturityLevelDashboardViewModelTemplate)
        {
            MaturityLevelDashboardViewModel = maturityLevelDashboardViewModelTemplate;
        }

        public async Task UpdateCharts()
        {
            if (MaturityLevelDashboardViewModel != null && MaturityLevelDashboardViewModel.Sections != null)
            {
                foreach (var categorySection in MaturityLevelDashboardViewModel.Sections)
                {
                    var categoryLevels = categorySection.QuestionCategoryLevels;
                    string chartName = "chart" + categorySection.PageNo;
                    var paramArray = new object[]
                    {
                    categoryLevels.Level1Percentage, categoryLevels.Level2Percentage,categoryLevels.Level3Percentage,categoryLevels.Level4Percentage ,chartName,categorySection.CategoryName

                    };
                    await JS.InvokeAsync<object>("drawChart", paramArray);
                }
            }
        }

        public async Task Init()
        {
            await InitCharts();
            StateHasChanged();
        }

        public void Update(MaturityLevelDashboardViewModel maturityLevelDashboardViewModel)
        {
            MaturityLevelDashboardViewModel = maturityLevelDashboardViewModel;
            StateHasChanged();
        }
    }
}
