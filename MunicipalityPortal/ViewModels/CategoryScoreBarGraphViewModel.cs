using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAWeb.ViewModels
{
    public class CategoryScoreBarGraphViewModel
    {
        public String Category { get; set; }
        public String ChartName
        {
            get
            {
                return Category + "Chart";
            }
        }
        public int Level1ScorePercent { get; set; }
        public int Level2ScorePercent { get; set; }
        public int Level3ScorePercent { get; set; }
        public int Level4ScorePercent { get; set; }
    }
}
