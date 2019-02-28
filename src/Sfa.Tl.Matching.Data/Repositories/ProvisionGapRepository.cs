using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProvisionGapRepository : AbstractBaseRepository<ProvisionGap>
    {

        public ProvisionGapRepository(ILogger<ProvisionGapRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<ProvisionGap> opportunities)
        {
            return await BaseCreateMany(opportunities);
        }
    }
}