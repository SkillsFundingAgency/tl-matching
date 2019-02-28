using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders
{
    public class SearchResultsBuilder
    {
        private readonly IList<ProviderVenueSearchResultDto> _searchResults;

        public SearchResultsBuilder()
        {
            _searchResults = new List<ProviderVenueSearchResultDto>
            {
                new ProviderVenueSearchResultDto
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
                new ProviderVenueSearchResultDto
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

        public IEnumerable<ProviderVenueSearchResultDto> Build() =>
            _searchResults;
    }
}
