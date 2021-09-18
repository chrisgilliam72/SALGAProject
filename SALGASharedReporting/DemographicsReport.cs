using SALGADBLib;
using SALGASharedReporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGASharedReporting
{
    public class DemographicsReport
    {
        public static async Task<HRMDTableViewModel> LoadDemographicsTableReport(Municipality municipality,IDemographicsRepository demographicsRepository)
        {
            var viewModel = new HRMDTableViewModel();
            var demographics = await demographicsRepository.GetDemographics(municipality);
            var hrDemographics = await demographicsRepository.GetHRMDDemographics(municipality);
            viewModel.MunicipalityName = municipality.Name;
            viewModel.TypeMunicipality = municipality.MunicipalCatagory.Catagory;
            if (demographics!=null)
            {
                viewModel.NoPeopleEmployed = demographics.NoEmployees;
                viewModel.TotalWageBill = demographics.TotalMonthlyPayroll;
                viewModel.Perm5657 = demographics.NoPerm54A56;
                viewModel.FixedTerm5657 = demographics.NoFixedTerm54A56;
                viewModel.PermNon5657 = demographics.NoPermNon54A56;
                viewModel.FixedTermNon5657 = demographics.NoFixedTermNon54A56;
                viewModel.Other = demographics.NoOther;
                if (hrDemographics!=null)
                {
                    viewModel.NoHRMDStaff = hrDemographics.NoPeople;
                    viewModel.HCMFunction = hrDemographics.CorporateService ? "Part of Corporate Services" : "Stand Alone Unit";
                }

            }
            return viewModel;
        }
    }
}
