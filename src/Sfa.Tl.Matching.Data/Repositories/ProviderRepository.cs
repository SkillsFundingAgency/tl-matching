using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderRepository : AbstractBaseRepository<Provider>
    {
        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        public override async Task<int> CreateMany(IEnumerable<Provider> providers)
        {
            return await BaseCreateMany(providers);
        }
        }

        {
        }
    }
}