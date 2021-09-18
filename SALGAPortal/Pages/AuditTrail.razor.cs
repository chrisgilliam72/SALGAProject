using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AuditTrail
    {
        [Inject]
        IAuditingRepository auditingRepository { get; set; }
        [Inject]
        IDemographicsRepository demographicsRepository { get; set; }
        public AuditResultsViewModel   AuditResults { get; set; }

        public List<String> MunicipalityList { get; set; }

        public String SelectedMunicipality { get; set; }

        public String ExcelDownloadString
        {
            get
            {
                return "/DownloadAuditExcelData?StartDate=" + AuditResults.StartDate + "&EndDate=" + AuditResults.EndDate +"&Municipality="+ SelectedMunicipality;
            }
        }
        
        public AuditTrail()
        {
            AuditResults = new AuditResultsViewModel();
            AuditResults.StartDate = DateTime.Today.AddDays(-7);
            AuditResults.EndDate = DateTime.Today.AddDays(1);

            MunicipalityList = new List<string>();
        }

        protected override async Task OnInitializedAsync()
        {
            var municipalities = await demographicsRepository.GetMunicipalities();
            MunicipalityList.AddRange(municipalities.Select(x => x.Name).OrderBy(x=>x));
            var auditEventsLst = await auditingRepository.GetAuditEvents(AuditResults.StartDate, AuditResults.EndDate);
            AuditResults.AuditEvents.AddRange(auditEventsLst.OrderByDescending(x=>x.Date).ToList());
        }

        public async Task OnApply()
        {
            AuditResults.AuditEvents.Clear();
            if (SelectedMunicipality==""  || SelectedMunicipality==null)
            {
                var auditEventsLst = await auditingRepository.GetAuditEvents(AuditResults.StartDate, AuditResults.EndDate);
                AuditResults.AuditEvents.AddRange(auditEventsLst.OrderByDescending(x => x.Date).ToList());
            }
            else
            {
                var auditEventsLst = await auditingRepository.GetAuditEvents(AuditResults.StartDate, AuditResults.EndDate,SelectedMunicipality);
                AuditResults.AuditEvents.AddRange(auditEventsLst.OrderByDescending(x => x.Date).ToList());
            }
            StateHasChanged();
        }

        public void OnExport()
        {

        }
    }
}
