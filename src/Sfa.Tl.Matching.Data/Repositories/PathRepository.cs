using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class PathRepository : AbstractBaseRepository<Path>
    {
        public PathRepository(ILogger<PathRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<Path> entities)
        {
            return await BaseCreateMany(entities);
        }
    }
}