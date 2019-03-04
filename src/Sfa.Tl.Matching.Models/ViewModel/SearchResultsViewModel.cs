using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchResultsViewModel
    {
        public IList<SearchResultsViewModelItem> Results { get; set; }
        //public IEnumerable<ProviderVenueSearchResultDto> Results { get; set; }
    }
}
