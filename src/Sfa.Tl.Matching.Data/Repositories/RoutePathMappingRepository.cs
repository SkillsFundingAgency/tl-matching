using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RoutePathMappingRepository : AbstractBaseRepository<RoutePathMapping>
    {
        private readonly MatchingDbContext _dbContext;

        public RoutePathMappingRepository(ILogger<RoutePathMappingRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IEnumerable<RoutePathMapping> routePathMappings)
        {
            return await BaseCreateMany(routePathMappings);
        }

        public override Task<RoutePathMapping> GetSingleOrDefault(Func<RoutePathMapping, bool> predicate)
        {
            return _dbContext.RoutePathMapping.SingleOrDefaultAsync(routePathMapping => predicate(routePathMapping));
        }
    }
}