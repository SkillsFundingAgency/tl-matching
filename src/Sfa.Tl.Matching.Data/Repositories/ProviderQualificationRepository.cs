using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderQualificationRepository : AbstractBaseRepository<ProviderQualification>
    {
        public ProviderQualificationRepository(ILogger<ProviderQualificationRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<ProviderQualification> entities)
        {
            return await BaseCreateMany(entities);
        }
    }
}