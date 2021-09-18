using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class ItemAuditTracking
    {
        public int pkID { get; set; }
        public String ItemType { get; set; }
        public String ItemName { get; set; }
        public String ItemPath { get; set; }
        public DateTime AccessDate { get; set; }
        public IdentityUser AccessedBy { get; set; }

        public String Email { get; set; }
  
    }

}
