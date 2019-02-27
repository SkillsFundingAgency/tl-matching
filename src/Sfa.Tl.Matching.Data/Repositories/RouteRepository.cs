using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class RouteRepository : AbstractBaseRepository<Route>
    {
        public RouteRepository(ILogger<RouteRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<Route> routes)
        {
            return await BaseCreateMany(routes);
        }
    }
}