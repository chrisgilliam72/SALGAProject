using Microsoft.AspNetCore.Components;
using SALGADBLib;
using SALGAPortal.ViewModels;
using SALGASharedReporting.ViewModels;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{


    public partial class ProvincialMap
    {

        [Parameter]
        public List<Province> Provinces { get; set; }
        [Parameter]
        public List<Municipality> Municipalities { get; set; }
        [Parameter]
        public EventCallback<String> OnProvinceSelected { get; set; }
        [Parameter]
        public List<String> AuthorizedProvinces { get; set; }

        public object MapShapeData;
        public List<MapProvinceMaturityLevel> MapColorData;

        private int NoMunicipalities { get; set; }
        private String SelectedProvince { get; set; }

        private CompletenessReportViewModel CompletenessReportViewModel { get; set; }
        private List<ProvinceAverageMaturityLevel> ProvinceAverageMaturityLevels { get; set; }

        public bool CanChooseAnyProvince { get; set; }


        public ProvincialMap()
        {
            AuthorizedProvinces = new List<string>();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            string fileText = System.IO.File.ReadAllText("wwwroot/MapData/South Africa.json");
            MapShapeData = Newtonsoft.Json.JsonConvert.DeserializeObject(fileText);

        }

        private void RefreshColors()
        {
            MapColorData = new List<MapProvinceMaturityLevel>();
            if (ProvinceAverageMaturityLevels != null && CompletenessReportViewModel!=null)
            {
                foreach (var provMaturityLvl in ProvinceAverageMaturityLevels)
                {
                    var provinceCompleteData = CompletenessReportViewModel.ProvinceRows.FirstOrDefault(x => x.Province == provMaturityLvl.ProvinceName);
                    var mapProvinceLevel = new MapProvinceMaturityLevel()
                    {
                        ProvinceName = provMaturityLvl.ProvinceName,
                        AverageMaturityLevel = "Maturity Level " + provMaturityLvl.AverageMaturityLevel,
                        AssessmentsCompleted = provinceCompleteData.CompleteMunicipalities,
                        AssessmentsNotCompleted = provinceCompleteData.IncompleteMunicipalities,
                        LegendVisibility = true

                    };
                    MapColorData.Add(mapProvinceLevel);
                }
            }
        }

        public void ShadeMapMaturityLevels(IEnumerable<ProvinceAverageMaturityLevel> provinceMaturityLevels, CompletenessReportViewModel completenessReport)
        {
            CompletenessReportViewModel = completenessReport;
            ProvinceAverageMaturityLevels = provinceMaturityLevels.ToList();

            RefreshColors();
            StateHasChanged();
        }

        async Task OnShapeSelected(ShapeSelectedEventArgs args)
        {
            var provDataElement = args.Data.FirstOrDefault(x => x.Key == "name");
            var province = provDataElement.Value;
            if (province!=null && Municipalities!=null && AuthorizedProvinces.Contains(province))
            {
                RefreshColors();
                var lstThisProvMunicipalities = Municipalities.Where(x => x.Province.Name == province).ToList();
                var mapProv = MapColorData.FirstOrDefault(x => x.ProvinceName == province);
                if (mapProv!=null)
                {
                    mapProv.AverageMaturityLevel = "selected";
                    NoMunicipalities = lstThisProvMunicipalities.Count();
                    SelectedProvince = province;
                    StateHasChanged();
                    await OnProvinceSelected.InvokeAsync(SelectedProvince);
                }


            }

        }
    
    }
}
