using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class AuthorizedPerson
    {
        [Parameter]
        public AuthorizedPersonViewModel AuthorizedPersonViewModel { get; set; }
        [Parameter]
        public EventCallback<AuthorizedPersonViewModel> OnUserDetailsUpdated { get; set; }

        [Inject]
        IDemographicsRepository demographicsRepository { get; set; }
        public async Task OnSave()
        {
            var details = await demographicsRepository.GetIntervieweeDetails(AuthorizedPersonViewModel.IntervieweeID);
            details.Email = AuthorizedPersonViewModel.Email;
            details.CellNumber = AuthorizedPersonViewModel.CellPhone;
            details.ContactNumber = AuthorizedPersonViewModel.Phone;
            details.Active = AuthorizedPersonViewModel.Active;
            await demographicsRepository.SaveInterviewDetails(details);

            await OnUserDetailsUpdated.InvokeAsync(AuthorizedPersonViewModel);
        }


    }
}
