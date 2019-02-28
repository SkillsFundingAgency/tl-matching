using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderVenueRepository : AbstractBaseRepository<ProviderVenue>
    {
        public ProviderVenueRepository(ILogger<ProviderVenueRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<ProviderVenue> entities)
        {
            return await BaseCreateMany(entities);
        }
    }
}