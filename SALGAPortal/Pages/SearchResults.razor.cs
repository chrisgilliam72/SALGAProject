using Microsoft.AspNetCore.Components;
using SALGAPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGAPortal.Pages
{
    public partial class SearchResults
    {
        [Parameter]
        public List<DocumentSearchResultViewModel> SearchResultList { get; set; }

        public SearchResults()
        {
            SearchResultList = new List<DocumentSearchResultViewModel>();
        }
    }
}
