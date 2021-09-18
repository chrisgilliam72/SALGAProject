using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SALGAWeb.Pages
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty()]
        public String Password { get; set; }
        [BindProperty()]
        public String ConfirmPassword { get; set; }

        private UserManager<IdentityUser> _userManager;
        public ChangePasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var currentUserID = _userManager.GetUserId(User);
            if (!String.IsNullOrEmpty(currentUserID))
            {
                if (ConfirmPassword ==Password)
                {
                    var user = await _userManager.FindByIdAsync(currentUserID);
                    if (user != null)
                    {
                        if (await _userManager.IsEmailConfirmedAsync(user))
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, Password);
                            if (result == IdentityResult.Success)
                               return RedirectToPage("/QuestionnaireStart");
                            else
                            {
                                foreach (var error in result.Errors)
                                    ModelState.AddModelError("", error.Description);
                                return Page();
                            }
                        }
                    } 
                    else
                    {
                        ModelState.AddModelError("", "Unable to change password email not confirmed.");
                        return Page();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Password and confirmation fields do not match");
                    return Page();
                }

              
            }

            ModelState.AddModelError("", "User not found");
            return Page();
        }
    }
}
