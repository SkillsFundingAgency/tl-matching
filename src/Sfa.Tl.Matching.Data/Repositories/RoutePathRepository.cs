using Sfa.Tl.Matching.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RoutePathRepository : IRoutePathRepository
    {
        private readonly MatchingDbContext _dbContext;

        public RoutePathRepository(MatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RoutePathLookup>> GetListAsync()
        {
            return await Task.FromResult(
                _dbContext.RoutePathLookup
                    .ToList());
        }
    }
}
