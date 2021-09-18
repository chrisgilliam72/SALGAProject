using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;

namespace SALGAWeb.Pages
{
    [Authorize]
    public class QuestionnaireStart : PageModel
    {
        private IDemographicsRepository _demographicsRepository;
        private UserManager<IdentityUser> _userManager;
        public bool IsReturningUser { get; set; }
        public bool CompletedDemographics { get; set; }
        public bool CompletedHRMDemographics { get; set; }
        public QuestionnaireStart(IDemographicsRepository demographicsRepository,
                                  UserManager<IdentityUser> userManager)
        {
            _demographicsRepository = demographicsRepository;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var userDetails = await _demographicsRepository.GetIntervieweeDetails(user);
                    IsReturningUser = userDetails.Onboarded;
                    var demographics = await _demographicsRepository.GetDemographics(user);
                    CompletedDemographics = demographics != null;
                    var hrmDemographics = await _demographicsRepository.GetHRMDDemographics(user);
                    CompletedHRMDemographics = hrmDemographics != null;
                }
            }
            catch (Exception ex)
            {
                RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }
    }
}
