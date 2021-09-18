using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsSharedModelLib
{
    public class Municipality
    {
        public int pkID { get; set; }
        public String Name { get; set; }


        public Municipality District { get; set; }
    }
}
