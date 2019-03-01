using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class QualificationRoutePathMappingRepository : GenericRepository<QualificationRoutePathMapping>
    {
        private readonly MatchingDbContext _dbContext;

        public QualificationRoutePathMappingRepository(ILogger<QualificationRoutePathMappingRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IList<QualificationRoutePathMapping> entities)
        {
            var mappingList = entities.ToList();

            //First Save All Mapping for Existing Qualification
            var recordCount = await base.CreateMany(mappingList.Where(mapping => mapping.QualificationId > 0).ToList());

            var list = mappingList.Where(mapping => mapping.Qualification != null)
                                  .GroupBy(mapping => mapping.Qualification, new QualificationEqualityComparer()).ToList();

            foreach (var grouping in list)
            {
                recordCount += await SaveQualificationAndProviderQualification(grouping);
            }

            return recordCount;
        }

        public async Task<int> SaveQualificationAndProviderQualification(IGrouping<Qualification, QualificationRoutePathMapping> qualificationMappingGroup)
        {
            //for each qualificationMappingGroup first save the qualification
            await _dbContext.AddAsync(qualificationMappingGroup.Key);

            await _dbContext.SaveChangesAsync();

            //for each item in qualificationMappingGroup update qualificationid and then save 
            await _dbContext.AddRangeAsync(qualificationMappingGroup.Select(mapping =>
            {
                mapping.Qualification = null;
                mapping.QualificationId = qualificationMappingGroup.Key.Id;
                return mapping;
            }));

            return await _dbContext.SaveChangesAsync();
        }
    }
}