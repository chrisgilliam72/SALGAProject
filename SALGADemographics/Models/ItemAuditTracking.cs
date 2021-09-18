using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class AuditEvent
    {
        public int pkID { get; set; }
        public String EventType { get; set; }
        public String ItemName { get; set; }
        public String ItemPath { get; set; }
        public DateTime Date { get; set; }
        public String UserIDString { get; set; }
        public String UserEmail { get; set; }

  
    }

}
