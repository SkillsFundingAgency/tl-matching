using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class SqlSearchProvider : ISearchProvider
    {
        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly MatchingDbContext _dbContext;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProviderVenueSearchResult>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId)
        {
            _logger.LogInformation($"Searching for providers within radius {searchRadius} of postcode '{postcode}' with route {routeId}");

            throw new NotImplementedException();
        }
    }
}
