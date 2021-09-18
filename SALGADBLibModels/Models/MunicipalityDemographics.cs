﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class MunicipalityDemographics
    {
        public int pkID { get; set; }
        public int NoEmployees { get; set; }
        public int NoPerm54A56 { get; set; }
        public int NoFixedTerm54A56 { get; set; }
        public int NoPermNon54A56 { get; set; }
        public int NoFixedTermNon54A56 { get; set; }
        public int NoOther { get; set; }
        public int TotalMonthlyPayroll { get; set; }
        public byte[] OrganogramImage { get; set; }
        public String OrganogramFileFormat { get; set; }

        public IdentityUser User { get; set; }
        public Municipality Municipality { get; set; }
        //public List<String> OtherInstitutions { get; set; }

        public Organogram Organogram { get; set; }

    }
}
