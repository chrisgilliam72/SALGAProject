using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class AuthorizedPersonViewModel
    {
        public int IntervieweeID { get; set; }
        public String Municipality { get; set; }
        public String Name { get; set; }

        public String Role { get; set; }

        public bool Active { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }

        public String CellPhone { get; set; }
    }
}
