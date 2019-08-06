using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchResultsViewModel
    {
        public int SearchResultProviderCount => Results?.Count ?? 0;
        public IList<SearchResultsViewModelItem> Results { get; set; }
        public IList<SearchResultsByRouteViewModelItem> AdditionalResults { get; set; }
    }
}
