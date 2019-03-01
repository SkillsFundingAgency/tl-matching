using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

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

        public async Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId)
        {
            _logger.LogInformation($"Searching for providers within radius {searchRadius} of postcode '{postcode}' with route {routeId}");

            return await Task.FromResult(new List<ProviderVenueSearchResultDto>());
        }
    }
}
