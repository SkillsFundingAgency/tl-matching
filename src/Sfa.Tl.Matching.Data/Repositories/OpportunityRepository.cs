using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class OpportunityRepository : AbstractBaseRepository<Opportunity>
    {

        public OpportunityRepository(ILogger<OpportunityRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<Opportunity> opportunities)
        {
            return await BaseCreateMany(opportunities);
        }
    }
}