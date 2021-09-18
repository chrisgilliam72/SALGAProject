using MailHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SALGADBLib;
using SALGAPortal.ViewModels;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{

    public partial class ProvincialAssessmentCompletion
    {
        [Inject]
        private IAssessmentRepository assessmentRepository { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        public IDemographicsRepository demographicsRepository { get; set; }
        [Inject]
        IConfiguration _configuration { get; set; }

        public ProvinceCompleteRow ProvinceData { get; set; }

        public AssessmentsCommunication AssessmentsCommunication { get; set; }

        public void Update(ProvinceCompleteRow newProvinceData)
        {
            ProvinceData = newProvinceData;
            StateHasChanged();
        }

        public void OnCommunication()
        {
            var checkedItems = ProvinceData.CheckedMunicipalities.Select(x => new AssessmentMunicipalityInfo { ID = x.ID, Name = x.Name }).ToList();
            AssessmentsCommunication.SetMunicipalityList(checkedItems);
        }

        public async Task OnSave()
        {
            var host = _configuration["Gmail:Host"];
            var port = int.Parse(_configuration["Gmail:Port"]);
            var username = _configuration["Gmail:Username"];
            var password = _configuration["Gmail:Password"];
            var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);
            var loginLink = _configuration["MunicipalPortalURL"];
            var questionSet = _configuration["QuestionSet"];
            var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var mailHelp = new SendMail(host, port, username, password, enable); 
            List<MunicipalityAssessmentConfig> assessmentConfigs = new List<MunicipalityAssessmentConfig>();
            var municipalities = await demographicsRepository.GetMunicipalities();
            var capturersLst = await demographicsRepository.GetAllCapturers();
            var municpalitIDs = AssessmentsCommunication.OpenAssessmentsViewModel.ToMunicipalities.Select(x => x.ID).ToList();
            List<string> emailAddressess = new List<string>();
  
            foreach (var assessmentMunicipalityInfo in AssessmentsCommunication.OpenAssessmentsViewModel.ToMunicipalities)
            {
                var assessmentConfig = new MunicipalityAssessmentConfig()
                {
                    Municipality = municipalities.First(x => x.pkID == assessmentMunicipalityInfo.ID),
                    OpenDate = AssessmentsCommunication.OpenAssessmentsViewModel.StartDate,
                    CloseDate = AssessmentsCommunication.OpenAssessmentsViewModel.EndDate,
                    CurrentQuestionSet = Convert.ToInt32(questionSet),
                    CurrentYear = AssessmentsCommunication.OpenAssessmentsViewModel.StartDate.Year
                };

                var capturer = capturersLst.FirstOrDefault(x => x.Municipality.pkID == assessmentMunicipalityInfo.ID);
                if (capturer != null && capturer.Email != null)
                        emailAddressess.Add(capturer.Email);
 
                assessmentConfigs.Add(assessmentConfig);
            }

            if (AssessmentsCommunication.OpenAssessmentsViewModel.NewAssessmeent)
                await assessmentRepository.AddAssessmentConfigs(assessmentConfigs);

            emailAddressess.Add("chrisg@daita.co.za");
            mailHelp.SendHTMLAsync(username, "salga@salga.co.za", emailAddressess, "", "MunicipalHRPulse assessment opened",
            AssessmentsCommunication.OpenAssessmentsViewModel.BodyText, rootDir + @"\wwwroot\EmailTemplates\assessment-open.html", loginLink, false);
        }
    }
}
