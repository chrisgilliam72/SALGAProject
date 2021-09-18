using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;
using SALGASharedReporting;
using SALGASharedReporting.ViewModels;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace MunicipalityPortal.Pages
{
    [Authorize]
    //https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/webkit
    public class WordReportModelPrinting : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public String MunicipalityName { get; set; }
        public Municipality Municipality { get; set; }
        public HRMDTableViewModel HRMDTableViewModel { get; set; }
        public MaturityLevelDashboardViewModel MaturityLevelDashboardViewModel { get; set; }
        public String TechnologyResponse { get; set; }

        public OverallMaturityLevels OverallMaturityLevels
        {
            get
            {
                return MaturityLevelDashboardViewModel.OverallMaturityLevels;
            }
        }
        private IDemographicsRepository _demographicsRepository;
        private IAssessmentRepository _assessmentRepository;

        public WordReportModelPrinting(IDemographicsRepository demographicsRepository, IAssessmentRepository assessmentRepository)
        {
            _demographicsRepository = demographicsRepository;
            _assessmentRepository = assessmentRepository;
            MaturityLevelDashboardViewModel = new MaturityLevelDashboardViewModel();
        }

        public async Task OnGet()
        {


            var muncipalities=  await _demographicsRepository.GetMunicipalities();
            Municipality = muncipalities.FirstOrDefault(x => x.Name.Trim() == MunicipalityName);
            if (Municipality!=null)
            {
                var config = await _assessmentRepository.GetAssessmentConfig(Municipality);
                TechnologyResponse = await _assessmentRepository.GetTechnologyInUse(Municipality, config.CurrentYear);
                HRMDTableViewModel = await DemographicsReport.LoadDemographicsTableReport(Municipality, _demographicsRepository);

                MaturityLevelDashboardViewModel = await MunicipalityDashboardReport.LoadMunicipalityReport(Municipality, config.CurrentYear, _assessmentRepository);
                MaturityLevelDashboardViewModel.SectionsMatrix.Build(MaturityLevelDashboardViewModel.GroupedLevelSections);
            }
          
        }

    }
}
