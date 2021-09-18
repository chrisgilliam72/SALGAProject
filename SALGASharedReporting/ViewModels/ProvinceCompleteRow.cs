using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SALGASharedReporting.ViewModels
{
    public class ProvinceCompleteRow
    {
        public String Province { get; set; }
        public int CompleteMunicipalities { get; set; }
        public int NoMunicipalities { get; set; }

        public int IncompleteMunicipalities
        {
            get
            {
                return NoMunicipalities - CompleteMunicipalities;
            }
        }

        public int PercentComplete
        {
            get
            {
                return Convert.ToInt32(((double)CompleteMunicipalities / (double)NoMunicipalities) * 100);
            }
        }

        public int PercentInomplete
        {
            get
            {
                return 100 - PercentComplete;
            }
        }

        public List<ProvincialMunicipalityCompletion> CheckedMunicipalities
        {
            get
            {
                var checkedList = new List<ProvincialMunicipalityCompletion>();
                checkedList.AddRange(MunicipalityCompleteList.Where(x => x.Selected));
                checkedList.AddRange(MunicipalityPartiallyCompleList.Where(x => x.Selected));
                checkedList.AddRange(MunicipalityNotStartedList.Where(x => x.Selected));
                return checkedList;
            }
        }

        public List<ProvincialMunicipalityCompletion> MunicipalityAllList
        {
            get
            {
                var AllList=  new List<ProvincialMunicipalityCompletion>();
                AllList.AddRange(MunicipalityCompleteList);
                AllList.AddRange(MunicipalityPartiallyCompleList);
                AllList.AddRange(MunicipalityNotStartedList);

                return AllList;
            }
        }

        public List<ProvincialMunicipalityCompletion> MunicipalityCompleteList { get; set; }
        public List<ProvincialMunicipalityCompletion> MunicipalityPartiallyCompleList { get; set; }
        public List<ProvincialMunicipalityCompletion> MunicipalityNotStartedList { get; set; }

        public ProvinceCompleteRow()
        {
            MunicipalityCompleteList = new List<ProvincialMunicipalityCompletion>();
            MunicipalityPartiallyCompleList = new List<ProvincialMunicipalityCompletion>();
            MunicipalityNotStartedList = new List<ProvincialMunicipalityCompletion>();

        }

    }
}
