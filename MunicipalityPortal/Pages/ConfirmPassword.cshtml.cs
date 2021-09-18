using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MunicipalityPortal.Pages
{
    public class ConfirmPasswordModel : PageModel
    {

        [BindProperty]
        [Required]
        public String Password { get; set; }
        [BindProperty]
        [Required]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public String Token { get; set; }
        [BindProperty]
        public String UserID { get; set; }
        private UserManager<IdentityUser> _userManager;

        public bool PasswordChanged { get; set; }

        public ConfirmPasswordModel(UserManager<IdentityUser> userManager)
        {
            PasswordChanged = false;
            _userManager = userManager;
        }

        public void OnGet(String userId,String token)
        {
            Token = token;
            UserID = userId;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Password != ConfirmPassword)
                {
                    ModelState.AddModelError("PasswordMissmatch", "Password and confirmation do not match");
                    return Page();

                }

                var user = await _userManager.FindByIdAsync(UserID);
                if (user != null)
                {

                    var result = await _userManager.ResetPasswordAsync(user, Token, Password);
                    if (result.Succeeded)
                    {
                        var isLockedOut = await _userManager.IsLockedOutAsync(user);
                        if (isLockedOut)
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        return RedirectToPage("/Login");
                    }

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
