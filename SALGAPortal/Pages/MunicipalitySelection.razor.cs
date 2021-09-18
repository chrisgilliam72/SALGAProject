using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using SALGADBLib;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class MunicipalitySelection
    {


 
        [Parameter]
        public List<Municipality> Municipalities { get; set; }
        [Parameter]
        public EventCallback<String> OnMunicipalitySelected { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        public List<MunicipalitySelectionIViewModel> ProvinceMunicipalities { get; set; }

        public String SelectedProvince { get; set; }



        protected override void OnInitialized()
        {
  
            ProvinceMunicipalities = new List<MunicipalitySelectionIViewModel>();
        }

        public void UpdateMuncipalityList(List<Municipality> municipalities)
        {
            ProvinceMunicipalities.Clear();
            foreach (var municipality in municipalities)
            {
                if (municipality.MunicipalCatagory.Catagory.ToLower() != "local")
                {
                    var viewModel = new MunicipalitySelectionIViewModel();
                    viewModel.MunicipalityName = municipality.Name;
                    viewModel.DisplayName = municipality.Name + " " + municipality.MunicipalCatagory.Catagory;
                    var locals = municipalities.Where(x => x.District == municipality).ToList();
                    viewModel.LocalMunicipalities = locals.Select(x => x.Name).ToList();
                    ProvinceMunicipalities.Add(viewModel);
                }

            }
        }


        public async Task OnMunicipalitySelectionChanged(String municipalityName)
        {
            await JS.InvokeAsync<object>("hideMunicipalityModal", new object[] { });
            await OnMunicipalitySelected.InvokeAsync(municipalityName);
        }
    }
}
