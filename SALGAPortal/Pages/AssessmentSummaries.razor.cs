using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SALGADBLib;
using SALGASharedReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AssessmentSummaries
    {



        [Inject]
        IAssessmentRepository _assessmentRepository { get; set; }
        [Inject]
        IDemographicsRepository _demographicsRepository { get; set; }
        [Inject]
        IDashboardPermissionsRepository _dashboardPermissionsRepository { get; set; }
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public List<String> ValueChainNames { get; set; }

        public List<AssessmentSummaryItemViewModel> SummaryItems { get; set; }

        public AssessmentSummaries()
        {
            ValueChainNames = new List<string>();
            SummaryItems = new List<AssessmentSummaryItemViewModel>();
        }

        protected override async Task OnInitializedAsync()
        {
            var ProvinceFilter = new List<String>();
            var allRoles = await _dashboardPermissionsRepository.GetAllRoles();
            var allProvinceRoes = await _dashboardPermissionsRepository.GetProvincialAccessList();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userRoleClaim = user.Claims.FirstOrDefault(x => x.Type == "roles");
            if (userRoleClaim != null)
            {
                var dbRole = allRoles.FirstOrDefault(x => x.RoleName == userRoleClaim.Value);
                var allowedProvinces = allProvinceRoes.Where(x => x.Role.pkID == dbRole.pkID).ToList();
                if (allowedProvinces != null && allowedProvinces.Count > 0)
                {
                    var allowedProvinceNames = allowedProvinces.Select(x => x.Province.Name);
                    ProvinceFilter.AddRange(allowedProvinceNames);
                }
            }



            var valueChains =  await _assessmentRepository.GetPageInfo();
            var municipalities  = await _demographicsRepository.GetMunicipalities();
            ValueChainNames.AddRange(valueChains.Select(x => x.Title));

            foreach (var municipality in municipalities)
            {
                var summaryItem = new AssessmentSummaryItemViewModel()
                {
                    MuncipalityName = municipality.Name,
                    Province = municipality.Province.Name,
                    Type = municipality.MunicipalCatagory.Catagory
                };

                SummaryItems.Add(summaryItem);
            }

            var maturityReport = new MunicipalityDashboardReport();
            var reports = await maturityReport.GetAllMaturityLevelReports(_assessmentRepository, _demographicsRepository);
            foreach (var report in reports)
            {
                var summaryItem = SummaryItems.First(x => x.MuncipalityName == report.Municipality.Name);
                foreach (var reportSection in report.Sections)
                    summaryItem.MaturityLevelValues.Add(reportSection.CategoryName, reportSection.MaturityLevel);

                summaryItem.OverallMaturityLevel = report.OverallMaturityLevels.MaturityLevel;
            }
   
            SummaryItems = SummaryItems.Where(x=> ProvinceFilter.Contains(x.Province)).OrderBy(x => x.MuncipalityName).ToList();
        

        }
    
       
    }
}
