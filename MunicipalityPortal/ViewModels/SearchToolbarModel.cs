using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityPortal.ViewModels
{
    public class SearchToolbarModel : PageModel
    {
        [BindProperty]
        public String SearchValue {get;set;}
    }
}
