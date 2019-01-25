using Sfa.Tl.Matching.Domain.Models;
using System.Linq;
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

        public IQueryable<Path> GetPaths()
        {
            return _dbContext.Path;
        }

        public IQueryable<Route> GetRoutes()
        {
            return _dbContext.Route;
        }
    }
}
