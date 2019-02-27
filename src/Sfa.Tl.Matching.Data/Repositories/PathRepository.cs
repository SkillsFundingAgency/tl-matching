using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class PathRepository : AbstractBaseRepository<Path>
    {
        public PathRepository(ILogger<PathRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        public override async Task<int> CreateMany(IEnumerable<Path> paths)
        {
            return await BaseCreateMany(paths);
        }
        }

        {
        }
    }
}