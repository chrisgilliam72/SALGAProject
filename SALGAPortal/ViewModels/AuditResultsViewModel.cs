using SALGADBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class AuditResultsViewModel
    {
        public String Municipality { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<AuditEvent> AuditEvents { get; set; }

        public AuditResultsViewModel()
        {
            AuditEvents = new List<AuditEvent>();
        }
    }
}
