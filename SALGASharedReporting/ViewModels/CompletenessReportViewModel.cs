using System;
using System.Collections.Generic;
using System.Text;

namespace SALGASharedReporting.ViewModels
{

    public class CompletenessReportViewModel
    {
        public List<ProvinceCompleteRow> ProvinceRows { get; set; }

        public CompletenessReportViewModel()
        {
            ProvinceRows = new List<ProvinceCompleteRow>();

        }
    }
}
