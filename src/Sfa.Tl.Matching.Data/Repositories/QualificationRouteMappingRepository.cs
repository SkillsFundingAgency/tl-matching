using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class QualificationRouteMappingRepository : GenericRepository<QualificationRouteMapping>
    {
        private readonly MatchingDbContext _dbContext;

        public QualificationRouteMappingRepository(ILogger<QualificationRouteMappingRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IList<QualificationRouteMapping> entities)
        {
            var mappingList = entities.ToList();

            //First Save All Mapping for Existing Qualification
            var recordCount = await base.CreateMany(mappingList.Where(mapping => mapping.QualificationId > 0).ToList());

            var list = mappingList.Where(mapping => mapping.Qualification != null)
                                  .GroupBy(mapping => mapping.Qualification, new QualificationEqualityComparer()).ToList();

            var qualificationRoutePathMappings = list.SelectMany(grouping => new List<QualificationRouteMapping>(grouping.ToList()).Select(mapping =>
            {
                mapping.Qualification = grouping.Key;
                return mapping;
            }));

            await _dbContext.AddRangeAsync(qualificationRoutePathMappings);

            recordCount += await _dbContext.SaveChangesAsync();

            return recordCount;
        }
    }
}