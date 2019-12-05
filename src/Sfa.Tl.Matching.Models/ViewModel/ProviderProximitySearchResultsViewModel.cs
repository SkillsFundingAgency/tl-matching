using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchResultsViewModel
    {
        public int SearchResultProviderCount => Results?.Count ?? 0;
        public IList<ProviderProximitySearchResultViewModelItem> Results { get; set; }
    }
}