using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class OpportunityProximitySearchResultsViewModel
    {
        public int SearchResultProviderCount => Results?.Count ?? 0;
        public IList<OpportunityProximitySearchResultViewModelItem> Results { get; set; }
        public IList<OpportunityProximitySearchResultByRouteViewModelItem> AdditionalResults { get; set; }
    }
}
