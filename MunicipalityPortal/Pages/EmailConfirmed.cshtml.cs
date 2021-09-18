using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SALGAWeb.Pages
{
    public class EmailConfirmedModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public String UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public String UserToken { get; set; }
        private UserManager<IdentityUser> _userManager;
        public String ConfirmMessage { get; set; }

        public EmailConfirmedModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(UserId);
                if (user != null)
                {

                    var result = await _userManager.ConfirmEmailAsync(user, UserToken);
                    if (result.Succeeded)
                        ConfirmMessage = "Your email has been confirmed";
                      
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }
                else
                    ModelState.AddModelError("", "Unable to reset password. User not found");

            }
            return Page();
        }
    }
}
