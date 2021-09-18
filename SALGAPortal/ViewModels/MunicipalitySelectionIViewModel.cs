using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class MunicipalitySelectionIViewModel
    {
        public String MunicipalityName { get; set; }
        public String DisplayName { get; set; }
        public List<String> LocalMunicipalities { get; set; }

        public bool IsLocal { get; set; }
        public bool HasLocalMunicipalities
        {
            get
            {
                return (LocalMunicipalities != null && LocalMunicipalities.Count >0);
            }
        }
    }
}
