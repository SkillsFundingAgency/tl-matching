using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RouteRepository : AbstractBaseRepository<Route>
    {
        private readonly MatchingDbContext _dbContext;

        public RouteRepository(ILogger<RouteRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IEnumerable<Route> routes)
        {
            return await BaseCreateMany(routes);
        }

        public override Task<IQueryable<Route>> GetMany(Func<Route, bool> predicate)
        {
            return Task.FromResult(_dbContext.Route.Where(route => predicate(route)));
        }

        public override Task<Route> GetSingleOrDefault(Func<Route, bool> predicate)
        {
            return _dbContext.Route.SingleOrDefaultAsync(route => predicate(route));
        }
    }
}