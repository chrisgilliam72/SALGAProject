﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SALGADBLib
{
    public class IntervieweeDetails
    {
        public int pkID { get; set; }

        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        public Municipality Municipality { get; set; }
        [Required]
        public String LineManager { get; set; }
        [Required]
        public int YearsInPosition { get; set; }
        public JobTitle JobTitle { get; set; }
        [Required]
        public DateTime InterviewDate { get; set; }
        [Required]
        public String ContactNumber { get; set; }
        [Required]
        public String CellNumber { get; set; }
        [Required]
        public String Email { get; set; }

        public IdentityUser User { get; set; }

    }
}
