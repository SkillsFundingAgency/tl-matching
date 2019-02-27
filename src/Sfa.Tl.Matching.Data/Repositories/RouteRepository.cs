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
        public RouteRepository(ILogger<RouteRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IEnumerable<Route> routes)
        {
            return await BaseCreateMany(routes);
        }
    }
}