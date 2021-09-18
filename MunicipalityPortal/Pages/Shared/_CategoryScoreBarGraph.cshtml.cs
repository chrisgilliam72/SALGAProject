using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SALGAWeb.Pages.Shared
{
    public class _CategoryScoreBarGraphModel : PageModel
    {
        public String Category { get; set; }
        public int Level1ScorePercent { get; set; }
        public int Level2ScorePercent { get; set; }
        public int Level3ScorePercent { get; set; }
        public int Level4ScorePercent { get; set; }
        public void OnGet()
        {
        }
    }
}
