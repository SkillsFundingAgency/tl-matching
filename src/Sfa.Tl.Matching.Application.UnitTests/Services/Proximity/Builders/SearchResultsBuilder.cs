using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Proximity.Builders
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
                    ProviderVenueId = 1,
                    ProviderName = "The WKCIC Group",
                    Postcode = "NW1 3HB",
                    Distance = 2.5d,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "health and physiotherapy"
                    }
                },
                new ProviderVenueSearchResultDto
                {
                    ProviderVenueId = 2,
                    ProviderName = "Lambeth College",
                    Postcode = "SW4 9BL",
                    Distance = 2.5d,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "cooking"
                    }
                }
            };
        }

        public IList<ProviderVenueSearchResultDto> Build() =>
            _searchResults;
    }
}
