using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders
{
    public class ValidQualificationRoutePathMappingListBuilder
    {
        public IList<Domain.Models.QualificationRoutePathMapping> Build() => new List<Domain.Models.QualificationRoutePathMapping>
        {
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = QualificationRoutePathMappingConstants.Id,
                RouteId = QualificationRoutePathMappingConstants.RouteId,
                Source = QualificationRoutePathMappingConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarsId = QualificationRoutePathMappingConstants.LarsId,
                    Title = QualificationRoutePathMappingConstants.Title,
                    ShortTitle = QualificationRoutePathMappingConstants.ShortTitle
                }
            },
            new Domain.Models.QualificationRoutePathMapping
            {
                Id = QualificationRoutePathMappingConstants.Id2,
                RouteId = QualificationRoutePathMappingConstants.RouteId2,
                Source = QualificationRoutePathMappingConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarsId = QualificationRoutePathMappingConstants.LarsId2,
                    Title = QualificationRoutePathMappingConstants.Title2,
                    ShortTitle = QualificationRoutePathMappingConstants.ShortTitle2
                }
            }
        };
    }
}
