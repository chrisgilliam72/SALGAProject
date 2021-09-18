using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityPortal.ViewModels
{
    public class DocumentSearchResultViewModel
    {
        public String Municipality { get; set; }
        public String DocumentName { get; set; }
        public String DocumentLink { get; set; }
        public String UserID { get; set; }
        public int EvidenceID { get; set; }
        public String HostURL { get; set; }

        public String DownloadURL
        {
            get
            {
                return HostURL + UserID+@"/"+EvidenceID;
            }
        }
    }
}
