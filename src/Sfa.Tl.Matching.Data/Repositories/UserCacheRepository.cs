using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class UserCacheRepository : GenericRepository<UserCache>, IUserCacheRepository
    {
        private readonly MatchingDbContext _dbContext;

        public UserCacheRepository(ILogger<UserCacheRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }
    }
}