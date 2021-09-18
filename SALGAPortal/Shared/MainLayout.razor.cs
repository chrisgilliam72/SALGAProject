using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SALGADBLib;
using SALGAPortal.ViewModels;

namespace SALGAPortal.Shared
{
    public partial class MainLayout
    {
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
        private string _authMessage;
        private String FullName { get; set; }
        private String Autorizationlevel { get; set; }
        private String SearchText { get; set; }

        private List<DocumentSearchResultViewModel> SearchResultsList { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        IAssessmentRepository AssessmentRepository { get; set; }

        public MainLayout()
        {
            SearchResultsList = new List<DocumentSearchResultViewModel>();
        }

        protected override async Task OnInitializedAsync()
        {

            await GetClaimsPrincipalData();
        }

        private async Task GetClaimsPrincipalData()
        {

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                _authMessage = $"{user.Identity.Name} is authenticated.";
                _claims = user.Claims;
                FullName = user.FindFirst(c => c.Type == "name")?.Value.ToString();
                var userRoleClaim = user.Claims.FirstOrDefault(x => x.Type == "roles");
                if (userRoleClaim != null)
                {
                    var ci = (ClaimsIdentity)user.Identity;
                    var claim = new Claim(ci.RoleClaimType, userRoleClaim.Value);
                    ci.AddClaim(claim);
                    Autorizationlevel = " - (" + userRoleClaim.Value + ")";
                }
                else
                    Autorizationlevel = "(missing role)";

            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }


        private async Task SearchClick()
        {
            SearchResultsList.Clear();

            var evidenceDocs = await AssessmentRepository.FindEvidenceDocuments(SearchText);
            foreach (var evidenceDoc in evidenceDocs)
            {
                var searchResult = new DocumentSearchResultViewModel()
                {
                    Municipality = evidenceDoc.Municipality.Name,
                    DocumentName = evidenceDoc.OriginalFileName,
                    DocumentLink = "/DownloadEvidenceFile?BlobName=" + evidenceDoc.BlobName

                };

                SearchResultsList.Add(searchResult);
            }
        }
    }
}

