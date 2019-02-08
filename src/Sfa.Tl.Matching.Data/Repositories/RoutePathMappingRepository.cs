using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RoutePathMappingRepository : IRepository<RoutePathMapping>
    {
        private readonly ILogger<RoutePathMappingRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public RoutePathMappingRepository(ILogger<RoutePathMappingRepository> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> CreateMany(IEnumerable<RoutePathMapping> routePathMappings)
        {
            _dbContext.RoutePathMapping.AddRange(routePathMappings);

            int createdRecordsCount;
            try
            {
                createdRecordsCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
            
            return createdRecordsCount;
        }

        public Task<RoutePathMapping> GetSingleOrDefault(Func<RoutePathMapping, bool> predicate)
        {
            return _dbContext.RoutePathMapping.SingleOrDefaultAsync(routePathMapping => predicate(routePathMapping));
        }
    }
}