using System;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionsSharedModelLib;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MailHelper;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO;

namespace AssessmentTimeNotifications
{
    public  class CheckAssessmentDates
    {
        private SALGADBContext _salgaDBContext;
        private String Host;
        private int Port;
        private String Username;
        private String Password;
     
        public CheckAssessmentDates(SALGADBContext salgaDBContext)
        {
            _salgaDBContext = salgaDBContext;
        }

        private bool SendHTML(string fromAddress, string fromName, List<String> toAddresses, string toName, string subject,
                    string additionalBody, string htmlFilePath, string clickLink, bool ccSender, ExecutionContext executionContext)
        {
            //try
            {

                var smtpClient = new SmtpClient
                {
                    Host = Host,
                    Port = Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(Username, Password)
                };



                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress);
                foreach (var address in toAddresses)
                    mailMessage.To.Add(address);

                if (ccSender)
                {
                    var ccAddress = new MailAddress(fromAddress);
                    mailMessage.CC.Add(ccAddress);
                }
                mailMessage.Subject = subject;
                //Fetching Email Body Text from EmailTemplate File.  
                StreamReader str = new StreamReader(htmlFilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                var newText = MailText.Replace("https://link.com", clickLink);

                newText = newText.Replace("[additionalbody]", additionalBody);

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(newText, null, MediaTypeNames.Text.Html);
                LinkedResource inline = new LinkedResource(executionContext.FunctionAppDirectory + "\\primarylogo.png", MediaTypeNames.Image.Jpeg);
                inline.ContentId = "salga_logo";

                avHtml.LinkedResources.Add(inline);
                mailMessage.AlternateViews.Add(avHtml);


                mailMessage.Body = newText;
                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);

                return true;
            }
            //catch (Exception ex)
            //{
            //    return false;
            //}

        }
        [FunctionName("CheckAssessmentDates")]
        public async Task Run([TimerTrigger("0 30 07 * * 0-6")]TimerInfo myTimer, ILogger log, ExecutionContext executionContext)
        {
            try
            {
                Host = Environment.GetEnvironmentVariable("MailHost");
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("MailPort"));
                Username = Environment.GetEnvironmentVariable("MailUsername");
                Password = Environment.GetEnvironmentVariable("MailPassword");
                var clickLinkUrl = Environment.GetEnvironmentVariable("SALGAPortalUrl");
                var mailStartTTLS = Convert.ToBoolean(Environment.GetEnvironmentVariable("MailStarttls"));

                var unsubmittedTrackings = await _salgaDBContext.AssessmentTrackings.Include(x => x.User).Include(x => x.Municipality).Where(x => x.IsSubmitted == false).ToListAsync();
                var municipalityIDS = unsubmittedTrackings.Select(x => x.Municipality.pkID).ToList();
                var configs = await _salgaDBContext.MunicipalityAssessmentConfigs.Include(x => x.Municipality).Where(x => municipalityIDS.Contains(x.Municipality.pkID)).ToListAsync();

               

                foreach (var tracking in unsubmittedTrackings)
                {
                    var configInfo = configs.Find(x => x.Municipality.pkID == tracking.Municipality.pkID);
                    var daysRemaining = (configInfo.CloseDate - DateTime.Today).TotalDays;


                    if (daysRemaining < 30 && daysRemaining > 0)
                    {
                        string daysremainingmsg = "you have " + daysRemaining + " days left to complete your assessment.";
                        SendHTML(Username, "chrisg@daita.co.za", new List<string> { tracking.User.Email, "chrisg@daita.co.za" }, "function test user", "municipalhr pulse portal assessment is due soon", daysremainingmsg,
                        "notification.html", clickLinkUrl, false, executionContext);
                    }
                    else if (daysRemaining < 1)
                    {
                        SendHTML(Username, "", new List<string> { tracking.User.Email,"chrisg@daita.co.za" }, "function test user", "municipalhr pulse portal assessment is due soon", "your assessment is overdue",
                        "notification.html", clickLinkUrl, false, executionContext);
                    }
                }

            }
            catch (Exception ex)
            {
                log.LogInformation($"C# Timer trigger function exception:"+ex.Message);
            }
     



            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
