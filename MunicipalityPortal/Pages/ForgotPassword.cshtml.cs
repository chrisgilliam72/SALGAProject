using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SALGAWeb.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty()]
        public String EmailAddress { get; set; }
        public bool EmailSent { get; set; }

        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        public  ForgotPasswordModel(UserManager<IdentityUser> userManager, IConfiguration configuration )
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var identyUser = await _userManager.FindByEmailAsync(EmailAddress);
            if (identyUser != null && await _userManager.IsEmailConfirmedAsync(identyUser))
            {
                var host = _configuration["Gmail:Host"];
                var port = int.Parse(_configuration["Gmail:Port"]);
                var username = _configuration["Gmail:Username"];
                var password = _configuration["Gmail:Password"];
                var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

                var token = await _userManager.GeneratePasswordResetTokenAsync(identyUser);
                var confirmationLink = Url.PageLink("ConfirmPassword",null, new { userId = identyUser.Id, token });

                var rootDir = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
                var mailHelp = new SendMail(host, port, username, password, enable);
                mailHelp.SendHTMLAsync(username, "", new List<string> { identyUser.Email }, "", "Municipal HR Pulse Portal Password Reset", "",
                    rootDir + @"\wwwroot\EmailTemplates\index-password-reset.html", confirmationLink, false);


                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError("", "EMail Address not found");
            }
            return Page();
        }
    }
}
