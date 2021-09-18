

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MailHelper
{
    public class SendMail
    {
        public String Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool StartTLS { get; set; }

        public SendMail(String host, int port, string userName, string passWord, bool tlsEnable)
        {
            Host = host;
            Port = port;
            UserName = userName;
            Password = passWord;
            StartTLS = tlsEnable;

        }

        public void SendAsync(string from, List<String> toAddresses, string subject, string content, bool ccSender)
        {
            Task.Run(() =>
            {
                Send(from, toAddresses, subject, content, ccSender);
            });
        }

        public void SendHTMLAsync(string from, List<String> toAddresses, string subject, string htmlFilePath, bool ccSender)
        {
            Task.Run(() =>
            {
                SendHTML(from,null, toAddresses,null, subject,null, htmlFilePath,null, ccSender);
            });
        }

        public void SendHTMLAsync(string fromAddress, string fromName, List<String> toAddresses, 
            string toName, string subject,string additionalBody, string htmlFilePath, string clickLink, bool ccSender)
        {
            Task.Run(() =>
            {
                SendHTML(fromAddress, fromName, toAddresses, toName, subject, additionalBody, htmlFilePath, clickLink, ccSender);
            });
        }

        public bool SendHTML(string fromAddress, string fromName, List<String> toAddresses, string toName, string subject,
                            string additionalBody,string htmlFilePath,string clickLink, bool ccSender)
        {
            //try
            {

                var smtpClient = new SmtpClient
                {
                    Host = Host,
                    Port = Port,
                    EnableSsl = StartTLS,
                    Credentials = new NetworkCredential(UserName, Password)
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

                newText = newText.Replace("Capturer_X", toName);
                newText = newText.Replace ("Approver_X", fromName);
                newText = newText.Replace("[additionalbody]", additionalBody);

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(newText, null, MediaTypeNames.Text.Html);
                LinkedResource inline = new LinkedResource(@".\wwwroot\EmailTemplates\primarylogo.png", MediaTypeNames.Image.Jpeg);
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

        public bool Send(string from, List<String> toAddresses, string subject, string content, bool ccSender)
        {
            try
            {

                var smtpClient = new SmtpClient
                {
                    Host = Host,
                    Port = Port,
                    EnableSsl =StartTLS,
                    Credentials = new NetworkCredential(UserName, Password)
                };

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from);
                foreach (var address in toAddresses)
                    mailMessage.To.Add(address);

                if (ccSender)
                {
                    var ccAddress = new MailAddress(from);
                    mailMessage.CC.Add(ccAddress);
                }
                mailMessage.Subject = subject;
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}



