using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;
using SALGASharedReporting;
using ClosedXML.Excel;
using System.IO;

namespace SALGAPortal.Pages
{
    public class DownloadSummaryReporExcelDataModel : PageModel
    {
        [Inject]
        IAssessmentRepository _assessmentRepository { get; set; }
        [Inject]
        IDemographicsRepository _demographicsRepository { get; set; }

        public DownloadSummaryReporExcelDataModel(IAssessmentRepository assessmentRepository, 
                                                    IDemographicsRepository demographicsRepository)
        {
            _assessmentRepository = assessmentRepository; 
            _demographicsRepository = demographicsRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var valueChains = await _assessmentRepository.GetPageInfo();
            var municipalities = await _demographicsRepository.GetMunicipalities();
            var valueChainNames= valueChains.Select(x => x.Title);
            List<AssessmentSummaryItemViewModel> SummaryItems = new List<AssessmentSummaryItemViewModel>();

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

            SummaryItems = SummaryItems.OrderBy(x => x.MuncipalityName).ToList();
            int totalCols = 1;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Audit Events");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Municipality";
                worksheet.Cell(currentRow, 2).Value = "Type";
                worksheet.Cell(currentRow, 3).Value = "Province";
                worksheet.Cell(currentRow, 4).Value = "Overall Maturity Level";
                int offset = 1;
                foreach (var valueChainName in valueChainNames)
                {
                    worksheet.Cell(currentRow, 4+offset).Value = valueChainName;
                    offset++;
                }
                totalCols = 4 + offset;

                offset = 1;
                foreach (var summaryItem in SummaryItems)
                {
                    offset = 1;
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = summaryItem.MuncipalityName;
                    worksheet.Cell(currentRow, 2).Value = summaryItem.Type;
                    worksheet.Cell(currentRow, 3).Value = summaryItem.Province;
                    worksheet.Cell(currentRow, 4).Value = summaryItem.OverallMaturityLevel;
                    foreach(var name in valueChainNames)
                    {
                        if(summaryItem.MaturityLevelValues.ContainsKey(name))
                        {
                            worksheet.Cell(currentRow, 4 + offset).Value = summaryItem.MaturityLevelValues[name];
                            offset++;
                        }

                    }
                }

                IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, totalCols+1).Address);
                range.Style.Fill.SetBackgroundColor(XLColor.SandyBrown);
                range.Style.Font.FontName = "Calibri";
                range.Style.Font.FontSize = 11;
                range.Style.Font.Bold = true;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Summary Maturity Profiles.xlsx");
                }
            }
        }
    }
}
