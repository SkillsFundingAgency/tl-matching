using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderRepository : AbstractBaseRepository<Provider>
    {
        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<Provider> entities)
        {
            return await BaseCreateMany(entities);
        }
    }
}