using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchResultsViewModel
    {
        public IEnumerable<ProviderVenueSearchResultDto> Results { get; set; }
    }
}
