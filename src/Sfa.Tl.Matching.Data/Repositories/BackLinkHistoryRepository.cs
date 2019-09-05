using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class BackLinkHistoryRepository : GenericRepository<BackLinkHistory>, IBackLinkRepository
    {
        private readonly UserCacheDbContext _dbContext;
        public BackLinkHistoryRepository(ILogger<BackLinkHistoryRepository> logger, UserCacheDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
