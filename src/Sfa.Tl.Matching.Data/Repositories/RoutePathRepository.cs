using Sfa.Tl.Matching.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RoutePathRepository : IRoutePathRepository
    {
        private readonly MatchingDbContext _dbContext;

        public RoutePathRepository(MatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Path>> GetPathsAsync()
        {
            return await _dbContext.Path.ToListAsync();
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync()
        {
            return await _dbContext.Route.ToListAsync();
        }
    }
}
