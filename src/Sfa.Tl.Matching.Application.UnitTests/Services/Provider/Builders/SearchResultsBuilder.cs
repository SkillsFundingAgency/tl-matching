using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class SearchResultsBuilder
    {
        private readonly IList<ProviderVenueSearchResult> _searchResults;

        public SearchResultsBuilder()
        {
            _searchResults = new List<ProviderVenueSearchResult>
            {
                new ProviderVenueSearchResult
                {
                    ProviderId = 1,
                    ProviderName = "The WKCIC Group",
                    Postcode = "NW1 3HB",
                    Distance = 2.5M,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "health and physiotherapy"
                    }
                },
                new ProviderVenueSearchResult
                {
                    ProviderId = 2,
                    ProviderName = "Lambeth College",
                    Postcode = "SW4 9BL",
                    Distance = 2.5M,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "cooking"
                    }
                }
            };
        }

        public IEnumerable<ProviderVenueSearchResult> Build() =>
            _searchResults;
    }
}
