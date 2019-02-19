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
        private readonly MatchingDbContext _dbContext;

        public PathRepository(ILogger<PathRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> Create(Path path)
        {
            return await BaseCreate(path);
        }

        public override async Task<int> CreateMany(IEnumerable<Path> paths)
        {
            return await BaseCreateMany(paths);
        }

        public override Task<IQueryable<Path>> GetMany(Func<Path, bool> predicate)
        {
            return Task.FromResult(_dbContext.Path.Where(path => predicate(path)));
        }

        public override Task<Path> GetSingleOrDefault(Func<Path, bool> predicate)
        {
            return _dbContext.Path.SingleOrDefaultAsync(path => predicate(path));
        }

        public override async Task Update(Path path)
        {
            await BaseUpdate(path);
        }
    }
}