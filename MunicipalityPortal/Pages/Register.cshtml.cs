using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using SALGADBLib;

namespace SALGAWeb.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty()]
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public String EmailAddress { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name = "Password")]
        public String Password { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name ="Confirm Password")]
        public String PasswordConfirm { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name = "Contact Number")]
        public String ContactNumber { get; set; }
        [Required]
        [BindProperty()]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [BindProperty()]
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Municipalities { get; set; }
        [BindProperty()]
        [Required]
        public String SelectedMunicipality { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name = "Role")]
        public String SelectedRole { get; set; }
        [BindProperty()]
        public List<SelectListItem> JobTitles { get; set; }
        [BindProperty()]
        [Required]
        [Display(Name = "Job Title")]
        public String SelectedJobTitle { get; set; }

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IConfiguration _configuration;
        private IDemographicsRepository _demographicsRepository;
        private IAuditingRepository _auditingRepository;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,
                                         IConfiguration configuration, IDemographicsRepository demographicsRepository, IAuditingRepository auditingRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _demographicsRepository = demographicsRepository;
            _roleManager = roleManager;
            _auditingRepository = auditingRepository;
        }

        public async Task OnGet()
        {
            await LoadDropDowns();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (Password != PasswordConfirm)
                    {
                        ModelState.AddModelError("PasswordMissmatch", "Password and confirmation do not match");
                        await LoadDropDowns();
                        return Page();

                    }

                    var dbMunicipalities = await _demographicsRepository.GetMunicipalities();
                    var municipality = dbMunicipalities.First(x => x.Name == SelectedMunicipality);

                    var demoDetails = await _demographicsRepository.GetIntervieweeDetails(municipality);
                    if (demoDetails != null)
                    {
                        
                        foreach (var userDetail in demoDetails)
                        {
                            if (await _userManager.IsInRoleAsync(userDetail.User, SelectedRole))
                            {
                                ModelState.AddModelError("", "Municipality "+SelectedRole+ " already registered");
                                await LoadDropDowns();
                                return Page();
                            }
                        }

                    }

                    var identityUser = new IdentityUser { UserName = EmailAddress, Email = EmailAddress };
                    var result = await _userManager.CreateAsync(identityUser, Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                        await LoadDropDowns();
                        return Page();
                    }

                    result = await _userManager.AddToRoleAsync(identityUser, SelectedRole);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                        await LoadDropDowns();
                        return Page();
                    }


                    var jobTitles = await _demographicsRepository.GetJobTitles();

                    JobTitles = jobTitles.Select(x => new SelectListItem { Value = x.Title, Text = x.Title }).OrderBy(x => x.Text).ToList();
                    Municipalities = dbMunicipalities.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).OrderBy(x => x.Text).ToList();

                    var inVwDetails = new IntervieweeDetails()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        CellNumber = ContactNumber,
                        JobTitle = jobTitles.FirstOrDefault(x => x.Title == SelectedJobTitle),
                        Municipality = dbMunicipalities.FirstOrDefault(x => x.Name == SelectedMunicipality),
                        LineManager = "",
                        ContactNumber = "",
                        YearsInPosition="",
                        User = identityUser,
                        Email = EmailAddress,
                        InterviewDate = DateTime.Today.AddYears(-5),
                        Onboarded = false,
                        Active=true
                    };


                    await _demographicsRepository.SaveInterviewDetails(inVwDetails);
                    await SendEmailConfirmation(identityUser);
                    await _auditingRepository.AddRegistrationEvent(identityUser, SelectedMunicipality);
                    return RedirectToPage("/RegistrationConfirmed");
                }

                else
                {
                    await LoadDropDowns();
                    return Page();
                }

            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { ExceptionString = ex.Message });
            }


        }

        private async Task LoadDropDowns()
        {
            var dbMunicipalities = await _demographicsRepository.GetMunicipalities();
            var jobTitles = await _demographicsRepository.GetJobTitles();

            JobTitles = jobTitles.Select(x => new SelectListItem { Value = x.Title, Text = x.Title }).OrderBy(x => x.Text).ToList();
            Municipalities = dbMunicipalities.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).OrderBy(x => x.Text).ToList();
            Roles = _roleManager.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
        }

        private async Task SendEmailConfirmation(IdentityUser identyUser)
        {
            var host = _configuration["Gmail:Host"];
            var port = int.Parse(_configuration["Gmail:Port"]);
            var username = _configuration["Gmail:Username"];
            var password = _configuration["Gmail:Password"];
            var enable = bool.Parse(_configuration["Gmail:SMTP:starttls:enable"]);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identyUser);
            var confirmationLink = Url.PageLink("/EmailConfirmed",pageHandler:null, new { UserId = identyUser.Id, UserToken= token } );

            var rootDir=_configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var mailHelp = new SendMail(host, port, username, password, enable);
            mailHelp.SendHTMLAsync(username, "", new List<string> { identyUser.Email }, "", "Municipal HR Pulse Portal Email Confirmation", "",
                rootDir + @"\wwwroot\EmailTemplates\index-email-verification.html", confirmationLink, false);
        }
    }
}
