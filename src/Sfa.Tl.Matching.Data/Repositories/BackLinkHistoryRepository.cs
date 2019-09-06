using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class BackLinkHistoryRepository : GenericRepository<BackLinkHistory>, IBackLinkRepository
    {
        private readonly MatchingDbContext _dbContext;
        public BackLinkHistoryRepository(ILogger<BackLinkHistoryRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
