using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SALGADBLib;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SALGAWeb.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        public String Email { get; set; }
        [BindProperty]
        [Required]
        public String Password { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }

        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IAuditingRepository _auditingRepository;
        private IDemographicsRepository _demographicsRepository;
        public LoginModel(UserManager<IdentityUser> userManager,
                         SignInManager<IdentityUser> signInManager,
                         IAuditingRepository auditingRepository,
                         IDemographicsRepository demographicsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auditingRepository = auditingRepository;
            _demographicsRepository = demographicsRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {

                    Claim claim = ((ClaimsIdentity)User.Identity).FindFirst("IsPersistent");
                    RememberMe = claim != null ? Convert.ToBoolean(claim.Value) : false;
                    var user = await _userManager.GetUserAsync(User);

                    bool isApprover = await _userManager.IsInRoleAsync(user, "Approver");
                    return RedirectToPage("/QuestionnaireStart");
                }
                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }
        }

        public async Task<ActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: true);

                    if (result == SignInResult.Success)
                    {
                        var user = await _userManager.FindByEmailAsync(Email);

                        if (user != null)
                        {
                            var details = await _demographicsRepository.GetIntervieweeDetails(user);
                            if (details==null)
                                ModelState.AddModelError("Error", "User details not found");
                            else
                            {
                                if (details.Active)
                                {
                                    await _auditingRepository.AddLoginEvent(user, details != null ? details.Municipality.Name : "");
                                    bool isApprover = await _userManager.IsInRoleAsync(user, "Approver");
                                    return RedirectToPage("/QuestionnaireStart");
                                }

                                ModelState.AddModelError("Error", "User deactivated");
                            }

                        }

                    }
                    else if (result == Microsoft.AspNetCore.Identity.SignInResult.NotAllowed)
                    {
                        ModelState.AddModelError("Error", "Email not confirmed");
                    }
                    else if (result == SignInResult.Failed)
                        ModelState.AddModelError("", "Invalid username or password");
                    else if (result == SignInResult.LockedOut)
                        ModelState.AddModelError("", "Account is locked out.");
                }


                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }

        }
    }
}
