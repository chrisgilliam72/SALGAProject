using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class HRMDFunctionDemographics
    {
       public int pkID { get; set; }
       public int NoPeople { get; set; }
       public bool CorporateService { get; set; }
       public IdentityUser User { get; set; }
       public Municipality Municipality { get; set; }
       public  MunicipalityOrganogram MunicipalityOrganogram { get; set; }
    }
}
