using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityPortal.ViewModels
{
    public class SeniorManagerDemographicsViewmodel
    {
        public int FilledPositionID { get; set; }
        public int ManagerID { get; set; }
        public String Portfolio { get; set; }
        public String Filledby { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DisplayOrder { get; set; }

        public bool CanEdit { get; set; }
        public SeniorManagerDemographicsViewmodel()
        {
            AppointmentDate = DateTime.Today;
        }

        public static explicit operator SeniorManagerDemographicsViewmodel(SeniorManager seniorManager)
        {
            var viewModel = new SeniorManagerDemographicsViewmodel()
            {
                ManagerID = seniorManager.pkID,
                Portfolio = seniorManager.Portfolio ,
                DisplayOrder = seniorManager.DisplayOrder,
                CanEdit= seniorManager.CanChange,
            };

            return viewModel;
        }

        public static explicit operator SeniorManagerDemographicsViewmodel(SeniorManagerPosition  seniorManagerPos)
        {
            var viewModel = new SeniorManagerDemographicsViewmodel()
            {
                ManagerID = seniorManagerPos.SeniorManager != null ? seniorManagerPos.SeniorManager.pkID : 0,
                DisplayOrder = seniorManagerPos.SeniorManager!=null ? seniorManagerPos.SeniorManager.DisplayOrder : 1,
                CanEdit = seniorManagerPos.SeniorManager!=null ? seniorManagerPos.SeniorManager.CanChange: false,
                Portfolio=seniorManagerPos.PortfolioDisplayValue,
                FilledPositionID = seniorManagerPos.pkID,
                Filledby= seniorManagerPos.Name,
                AppointmentDate= seniorManagerPos.AppointmentDate,
                
               
 
            };

            return viewModel;
        }

    }
}
