using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class Personnel
    {
        [Inject]
        private IDemographicsRepository demographicsRepository { get; set; }

        public List<AuthorizedPersonViewModel> ListAuthorizedUsers { get; set; }

        public Personnel()
        {
            ListAuthorizedUsers = new List<AuthorizedPersonViewModel>();
        }

        protected override async Task OnInitializedAsync()
        {
            //var capturerUsers = _userManager.GetUsersInRoleAsync("Capturer");
            //var approverUsers = _userManager.GetUsersInRoleAsync("Approver");

            var userRoles=  await demographicsRepository.GetAllUserRoles();
            var municipalities = await demographicsRepository.GetMunicipalities();
            var personnelData = await demographicsRepository.GetAllPersonnelData();

            foreach (var interviewDetails in personnelData)
            {
                var vmUser = new AuthorizedPersonViewModel();
                vmUser.Name = interviewDetails.FirstName + " " + interviewDetails.LastName;
                vmUser.Municipality = interviewDetails.Municipality.Name;
                vmUser.Role = userRoles.First(x => x.UserID == interviewDetails.User.Id).RoleName;
                vmUser.Email = interviewDetails.User.Email;
                vmUser.IntervieweeID = interviewDetails.pkID;
                vmUser.CellPhone = interviewDetails.CellNumber;
                vmUser.Phone = interviewDetails.ContactNumber;
                vmUser.Active = true;
                ListAuthorizedUsers.Add(vmUser);
            }
            StateHasChanged();
        }

        public void UpdateUserInList(AuthorizedPersonViewModel authorizedPersonViewModel)
        {
            var listUser = ListAuthorizedUsers.First(x => x.IntervieweeID == authorizedPersonViewModel.IntervieweeID);
            listUser.Email = authorizedPersonViewModel.Email;
            listUser.Phone = authorizedPersonViewModel.Phone;
            listUser.CellPhone = authorizedPersonViewModel.CellPhone;
            listUser.Phone = authorizedPersonViewModel.Phone;
            listUser.Active = authorizedPersonViewModel.Active;
            StateHasChanged();
        }
    }
}
