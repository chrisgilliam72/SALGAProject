using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class AssessmentTracking
    {
        public int pkID { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public IdentityUser Approver { get; set; }
        public IdentityUser User { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? SALGAReviewDate { get; set; }
        public bool SALGAApproved { get; set; }
        public bool SALGARejected { get; set; }

    }
}
