using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityPortal.ViewModels
{
    public class SALGAPageModel : PageModel
    {
        protected IAssessmentRepository _assessmentRepository;
        protected IDemographicsRepository _demographicsRepository;
        protected UserManager<IdentityUser> _userManager;

        public SALGAPageModel(IDemographicsRepository demographicsRepository,
                              IAssessmentRepository assessmentRepository, UserManager<IdentityUser> userManager)
        {
            _assessmentRepository = assessmentRepository;
            _demographicsRepository = demographicsRepository;
            _userManager = userManager;

        }
    }
}
