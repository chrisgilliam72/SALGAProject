using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.ViewModels
{
    public class MapProvinceMaturityLevel
    {
        public String ProvinceName { get; set; }
        public String AverageMaturityLevel { get; set; }

        public int AssessmentsCompleted { get; set; }

        public int AssessmentsNotCompleted { get; set; }

        public bool LegendVisibility { get; set; }

        public bool IsSelected { get; set; }

        public String Color
        {
            get
            {
                if (IsSelected)
                {
                    return "LightGray";
                }
                else
                {
                    switch (AverageMaturityLevel)
                    {
                        case "Maturity Level 0": return "Crimson";
                        case "Maturity Level 1": return "FireBrick"; 
                        case "Maturity Level 2": return "Coral"; 
                        case "Maturity Level 3": return "GreenYellow";
                        case "Maturity Level 4": return "DarkGreen";
                        default: return "white";
                    }
                }
            }
                 
        }
    }
}
