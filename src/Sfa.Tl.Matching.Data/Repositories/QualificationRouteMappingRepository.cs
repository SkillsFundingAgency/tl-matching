﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class QualificationRouteMappingRepository : GenericRepository<QualificationRouteMapping>
    {
        public QualificationRouteMappingRepository(ILogger<QualificationRouteMappingRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            
        }

        public override async Task<int> CreateManyAsync(IList<QualificationRouteMapping> entities)
        {
            var mappingList = entities.ToList();

            //First Save All Mapping for Existing Qualification
            var recordCount = await base.CreateManyAsync(mappingList.Where(mapping => mapping.QualificationId > 0).ToList());

            var list = mappingList.Where(mapping => mapping.Qualification != null)
                                  .GroupBy(mapping => mapping.Qualification, new QualificationEqualityComparer()).ToList();

            var qualificationRouteMappings = list.SelectMany(grouping => new List<QualificationRouteMapping>(grouping.ToList()).Select(mapping =>
            {
                mapping.Qualification = grouping.Key;
                return mapping;
            }));

            await _dbContext.AddRangeAsync(qualificationRouteMappings);

            recordCount += await _dbContext.SaveChangesAsync();

            return recordCount;
        }
    }
}