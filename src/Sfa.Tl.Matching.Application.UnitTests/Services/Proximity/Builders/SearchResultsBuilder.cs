using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Proximity.Builders
{
    public class SearchResultsBuilder
    {
        private readonly IList<SearchResultsViewModelItem> _searchResults;

        public SearchResultsBuilder()
        {
            _searchResults = new List<SearchResultsViewModelItem>
            {
                new SearchResultsViewModelItem
                {
                    ProviderVenueId = 1,
                    ProviderName = "The WKCIC Group",
                    ProviderVenuePostcode = "NW1 3HB",
                    Distance = 2.5d,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "health and physiotherapy"
                    }
                },
                new SearchResultsViewModelItem
                {
                    ProviderVenueId = 2,
                    ProviderName = "Lambeth College",
                    ProviderVenuePostcode = "SW4 9BL",
                    Distance = 2.5d,
                    QualificationShortTitles = new List<string>
                    {
                        "applied science",
                        "cooking"
                    }
                }
            };
        }

        public IList<SearchResultsViewModelItem> Build() =>
            _searchResults;
    }
}
