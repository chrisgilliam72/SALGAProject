using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class Municipality
    {
        public int pkID { get; set; }
        public String Name { get; set; }
        public MunicipalCatagory MunicipalCatagory { get; set; }
        public Province Province { get; set; }

        public Municipality District { get; set; }
    }
}
