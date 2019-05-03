using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchResultsViewModel
    {
        public int SearchResultProviderCount => Results?.Count ?? 0;
        public IList<ProviderSearchResultItemViewModel> Results { get; set; }
    }
}