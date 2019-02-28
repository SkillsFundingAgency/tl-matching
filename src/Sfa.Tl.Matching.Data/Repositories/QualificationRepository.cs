using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class QualificationRepository : AbstractBaseRepository<Qualification>
    {
        public QualificationRepository(ILogger<QualificationRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<int> CreateMany(IList<Qualification> entities)
        {
            //First Save All Mapping for Existing Qualification
            return await BaseCreateMany(entities);
        }
    }
}