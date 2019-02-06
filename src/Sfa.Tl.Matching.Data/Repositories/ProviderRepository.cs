using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        private readonly ILogger<ProviderRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> CreateMany(IEnumerable<Provider> providers)
        {
            _dbContext.Provider.AddRange(providers);
            
            int createdRecordsCount;
            try
            {
                createdRecordsCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return createdRecordsCount;
        }

        public async Task<Provider> GetSingleOrDefault(Func<Provider, bool> predicate)
        {
            return await _dbContext.Provider.SingleOrDefaultAsync(p => predicate(p));
        }
    }
}