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
        private readonly MatchingDbContext _dbContext;

        public ProviderVenueRepository(ILogger<ProviderVenueRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> Create(ProviderVenue providerVenue)
        {
            return await BaseCreate(providerVenue);
        }

        public override async Task<int> CreateMany(IEnumerable<ProviderVenue> providerVenues)
        {
            return await BaseCreateMany(providerVenues);
        }

        public override Task<IQueryable<ProviderVenue>> GetMany(Func<ProviderVenue, bool> predicate)
        {
            return Task.FromResult(_dbContext.ProviderVenue.Where(providerVenue => predicate(providerVenue)));
        }

        public override async Task<ProviderVenue> GetSingleOrDefault(Func<ProviderVenue, bool> predicate)
        {
            return await _dbContext.ProviderVenue.SingleOrDefaultAsync(p => predicate(p));
        }

        public override async Task Update(ProviderVenue providerVenue)
        {
            await Update(providerVenue);
        }
    }
}