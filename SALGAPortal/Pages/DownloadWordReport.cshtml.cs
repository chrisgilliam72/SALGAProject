using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace SALGAPortal.Pages
{
    public class DownloadWordReportModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public String MunicipalityName { get; set; }
        private IConfiguration _configuration;

        public DownloadWordReportModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult OnGet()
        {
            var currentYear = DateTime.Today.Year;
            var blobMetaData = new Dictionary<string, string>();


            string cookieName = ".AspNetCore.Cookies";
            string cookieValue = string.Empty;
            if (Request.Cookies[cookieName] != null)
            {
                cookieValue = Request.Cookies[cookieName];
            }
            //Initialize HTML converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            //WebKit converter settings
            WebKitConverterSettings webKitSettings = new WebKitConverterSettings();
            var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            //Assign the WebKit binaries path
            webKitSettings.WebKitPath = rootDir + @"\wwwroot\QtBinaries";
            webKitSettings.EnableJavaScript = true;
            webKitSettings.EnableToc = true;
            webKitSettings.EnableHyperLink = true;
            webKitSettings.AdditionalDelay = 5000;
            //Add cookies as name and value pair
            webKitSettings.Cookies.Add(cookieName, cookieValue);
            //Assign the WebKit settings
            htmlConverter.ConverterSettings = webKitSettings;
            //Convert URL to PDF
            var pageLink = Url.PageLink("WordReportPrinting",null, new { MunicipalityName = MunicipalityName });
            //PdfDocument document = htmlConverter.Convert(reportLink);
            //PdfDocument document = htmlConverter.Convert("www.news24.com");
            PdfDocument document = htmlConverter.Convert(pageLink);
            //Save the document into stream.
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            stream.Position = 0;

            //Close the document.
            document.Close(true);

            return File(stream, "application/pdf", MunicipalityName + " Municipal HR Pulse Report.pdf");

        }

    }
}
