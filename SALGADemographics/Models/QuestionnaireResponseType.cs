﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SALGADBLib
{
    public class QuestionnaireResponseType
    {
        public int pkID { get; set; }
        public String ResponseType { get; set; }

        public bool VisibleToUser { get; set; }
        public bool VisibleToApprover { get; set; }
    }
}
