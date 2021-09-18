using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;

namespace SALGAPortal.Pages
{
    public class DownloadAuditExcelData : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public String Municipality { get; set; }

        [BindProperty(SupportsGet =true)]
        public DateTime StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        private readonly IWebHostEnvironment _env;
        private readonly IAuditingRepository _auditingRepository;

        public DownloadAuditExcelData(IWebHostEnvironment env, IAuditingRepository auditingRepository)
        {
            _env = env;
            _auditingRepository = auditingRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            List<AuditEvent> auditEventsLst = null;

            if (String.IsNullOrEmpty(Municipality))
            {
                auditEventsLst = await _auditingRepository.GetAuditEvents(StartDate, EndDate);
            }
            else
            {
                auditEventsLst = await _auditingRepository.GetAuditEvents(StartDate, EndDate, Municipality);
            }



            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Audit Events");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Event ID";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Municipality";
                worksheet.Cell(currentRow, 4).Value = "Event Type";
                worksheet.Cell(currentRow, 5).Value = "Event Path";
                worksheet.Cell(currentRow, 6).Value = "User";
                foreach (var auditEvent in auditEventsLst)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = auditEvent.pkID;
                    worksheet.Cell(currentRow, 2).Value = auditEvent.Date;
                    worksheet.Cell(currentRow, 3).Value = auditEvent.ItemName;
                    worksheet.Cell(currentRow, 4).Value = auditEvent.EventType;
                    worksheet.Cell(currentRow, 5).Value = auditEvent.ItemPath;
                    worksheet.Cell(currentRow, 6).Value = auditEvent.UserEmail;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "audittrail.xlsx");
                }
            }

        }
    }
}
