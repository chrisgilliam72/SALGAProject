using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SALGADBLib;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class Index
    {
        [Inject]
        private IDemographicsRepository _demographicsRepository { get; set; }
        [Inject]
        private IAssessmentRepository _assessmentRepository { get; set; }
        [Inject]
        private IDashboardPermissionsRepository _dashboardPermissionsRepository { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public Municipality SelectedMunicipality { get; set; }
        public List<Municipality> Municipalities { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Municipality> ProvinceMunicipalities { get; set; }
        public List<String> AuthorizedProvinceNames { get; set; }
        private List<QuestionnaireResponseType> ResponseTypes { get; set; }

        String SelectedProvince { get; set; }

        ProvincialMap SyncFusionMap { get; set; }

        MunicipalitySelection MunicipalitySelection { get; set; }

        MaturityLevelSummary MaturityLevelSummary { get; set; }
        MaturityLevelsDashboard MaturityLevelsDashboard { get; set; }
        ValueChainBreakdown ValueChainBreakdown { get; set; }

        AssessmentSections AssessmentSections { get; set; }
        PreviousAssessments PreviousAssessments { get; set; }
        public OverallMaturityLevels OverallMaturityLevels { get; set; }
        public int CurrentMaturityLeval { get; set; }
        private bool HasData { get; set; }
        public String SortByMethod { get; set; }


        public Index()
        {
            AuthorizedProvinceNames = new List<string>();
        }

        protected override async Task OnInitializedAsync()
        {
          

            var currentYear = DateTime.Now.Year;
            var mapReports = new MapReports();

            AuthorizedProvinceNames.Clear();

            ResponseTypes=(await _assessmentRepository.GetResponseTypes()).ToList();
            var allRoles=await _dashboardPermissionsRepository.GetAllRoles();
            var allProvinceRoes = await _dashboardPermissionsRepository.GetProvincialAccessList();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userRoleClaim = user.Claims.FirstOrDefault(x => x.Type == "roles");
            if (userRoleClaim!=null)
            {
                var dbRole = allRoles.FirstOrDefault(x => x.RoleName == userRoleClaim.Value);
                var allowedProvinces = allProvinceRoes.Where(x => x.Role.pkID == dbRole.pkID).ToList();
                if (allowedProvinces != null && allowedProvinces.Count > 0)
                {
                    var allowedProvinceNames = allowedProvinces.Select(x => x.Province.Name);
                    AuthorizedProvinceNames.AddRange(allowedProvinceNames);
                }
            }

            var avMaturityLevels = await mapReports.AverageMaturityLevelPerProvince(currentYear, _assessmentRepository, _demographicsRepository);
            var completenessReport = await AssessmentManagementReport.GetCompletenessReport(_assessmentRepository, _demographicsRepository, currentYear);

            SelectedMunicipality = null;
            Municipalities = (await _demographicsRepository.GetMunicipalities()).OrderBy(x => x.Name).ToList();

            Provinces = Municipalities.Select(x => x.Province).Distinct().OrderBy(x => x.Name).ToList();
            if (SyncFusionMap != null)
                SyncFusionMap.ShadeMapMaturityLevels(avMaturityLevels, completenessReport);
            var maturityLevelDashboardViewModel = await MunicipalityDashboardReport.LoadTemplate(_assessmentRepository);
            if (MaturityLevelsDashboard != null)
                MaturityLevelsDashboard.SetTemplate(maturityLevelDashboardViewModel);
            StateHasChanged();
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {

           if (!firstRender)
            {

                if (MaturityLevelsDashboard != null)
                    await MaturityLevelsDashboard.Init();

                if (OverallMaturityLevels != null)
                {
                    var paramArray = new object[]
                    {
                    OverallMaturityLevels.Level1Percent, OverallMaturityLevels.Level2Percent,OverallMaturityLevels.Level3Percent,OverallMaturityLevels.Level4Percent
                    ,"chartOverall","HRM&D Maturity Level"

                    };

                    await JS.InvokeAsync<object>("drawChart", paramArray);
                }
                await JS.InvokeAsync<object>("hideBusyModel", null);
            }


        }

        protected async Task ProvinceSelected(String selectedProvince)
        {
            SelectedProvince = selectedProvince;
            var selProvince = Provinces.FirstOrDefault(x => x.Name == SelectedProvince);
            var lstThisProvMunicipalities = Municipalities.Where(x => x.Province == selProvince).ToList();
            MunicipalitySelection.UpdateMuncipalityList(lstThisProvMunicipalities);
            await JS.InvokeAsync<object>("showMunicipalityModal",new object[] { });
     
        }

        protected async Task MunicipalityReportUpdate(String municipalityName)
        {

            SelectedMunicipality = Municipalities.FirstOrDefault(x => x.Name == municipalityName);
            var config = await _assessmentRepository.GetAssessmentConfig(SelectedMunicipality);
            if (config!=null)
            {
                await JS.InvokeAsync<object>("showBusyModel", null);
                HasData = true;
                var maturityLevelDashboardViewModel = await MunicipalityDashboardReport.LoadMunicipalityReport(SelectedMunicipality, config.CurrentYear, _assessmentRepository);
                CurrentMaturityLeval = maturityLevelDashboardViewModel.OverallMaturityLevels.MaturityLevel;
                OverallMaturityLevels = maturityLevelDashboardViewModel.OverallMaturityLevels;
                MaturityLevelsDashboard.Update(maturityLevelDashboardViewModel);
                MaturityLevelSummary.Update(maturityLevelDashboardViewModel);
                ValueChainBreakdown.Update(maturityLevelDashboardViewModel);
                await AssessmentSections.Update(SelectedMunicipality);
                PreviousAssessments.Update(SelectedMunicipality);
            }
            else
            {
                HasData = false;
                MaturityLevelsDashboard.Update(null);
                MaturityLevelSummary.Update(null);
                ValueChainBreakdown.Update(null);
                await AssessmentSections.Update(null);
                PreviousAssessments.Update(null);
            }
            StateHasChanged();
        }
    }
}
