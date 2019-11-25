using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderProximity.Builders
{
    public class SearchResultsBuilder
    {
        public IList<SearchResultsViewModelItem> Build() =>
            new List<SearchResultsViewModelItem>
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
}
