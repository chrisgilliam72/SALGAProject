using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{
    public class HRMDTableViewModel
    {
        public String MunicipalityName { get; set; }
        public String TypeMunicipality { get; set; }
        public int NoPeopleEmployed { get; set; }
        public double TotalWageBill { get; set; }
        public int NoHRMDStaff { get; set; }
        public String HCMFunction { get; set; }
        public int Perm5657 { get; set; }
        public int FixedTerm5657 { get; set; }
        public int PermNon5657 { get; set; }
        public int FixedTermNon5657 { get; set; }

        public int Other { get; set; }

        public decimal HRRatio
        {
            get
            {
                return NoPeopleEmployed != 0 ? NoPeopleEmployed/ NoHRMDStaff   : 0;
            }
        }

        public double AverageMonthlyWagePP
        {
            get
            {
                return Math.Round(TotalWageBill != 0 ? TotalWageBill/NoPeopleEmployed  : 0,2);
            }
        }

    }
}
