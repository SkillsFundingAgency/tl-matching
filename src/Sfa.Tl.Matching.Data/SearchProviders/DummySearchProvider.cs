using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class DummySearchProvider : ISearchProvider
    {
        private readonly ILogger<DummySearchProvider> _logger;

        public DummySearchProvider(ILogger<DummySearchProvider> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<ProviderVenueSearchResult>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId)
        {
            _logger.LogInformation($"Searching for providers within radius {searchRadius} of postcode '{postcode}' with route {routeId}");

            var items = new List<ProviderVenueSearchResult>();

            if (postcode == "SW1A 2AA")
            {
                items.Add(new ProviderVenueSearchResult
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
                });
                items.Add(new ProviderVenueSearchResult
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
                });
            }

            return Task.FromResult<IEnumerable<ProviderVenueSearchResult>>(items);
        }
    }
}
