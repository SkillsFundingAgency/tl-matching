using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderVenueRepository : AbstractBaseRepository<ProviderVenue>
    {
        public ProviderVenueRepository(ILogger<ProviderVenueRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        public override async Task<int> CreateMany(IEnumerable<ProviderVenue> providerVenues)
        {
            return await BaseCreateMany(providerVenues);
        }
        }

        {
        }
    }
}