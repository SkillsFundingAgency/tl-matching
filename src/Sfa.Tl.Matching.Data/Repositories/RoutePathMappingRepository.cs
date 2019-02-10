using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<IQueryable<RoutePathMapping>> GetMany(Func<RoutePathMapping, bool> predicate)
        {
            return Task.FromResult(_dbContext.RoutePathMapping.Where(routePathMapping => predicate(routePathMapping)));
        }

        public Task<RoutePathMapping> GetSingleOrDefault(Func<RoutePathMapping, bool> predicate)
        {
            return _dbContext.RoutePathMapping.SingleOrDefaultAsync(routePathMapping => predicate(routePathMapping));
        }
    }
}