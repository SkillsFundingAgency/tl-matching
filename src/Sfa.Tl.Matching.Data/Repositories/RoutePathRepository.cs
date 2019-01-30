using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

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
            return _dbContext.Path
                .Include("Route")
                .AsNoTracking();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _dbContext.Route;
        }
    }
}