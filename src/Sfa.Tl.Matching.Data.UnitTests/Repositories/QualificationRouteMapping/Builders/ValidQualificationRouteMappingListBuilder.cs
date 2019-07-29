using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders
{
    public class ValidQualificationRouteMappingListBuilder
    {
        public IList<Domain.Models.QualificationRouteMapping> Build() => new List<Domain.Models.QualificationRouteMapping>
        {
            new Domain.Models.QualificationRouteMapping
            {
                Id = QualificationRouteMappingConstants.Id,
                RouteId = QualificationRouteMappingConstants.RouteId,
                Source = QualificationRouteMappingConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarsId = QualificationRouteMappingConstants.LarsId,
                    Title = QualificationRouteMappingConstants.Title,
                    ShortTitle = QualificationRouteMappingConstants.ShortTitle
                }
            },
            new Domain.Models.QualificationRouteMapping
            {
                Id = QualificationRouteMappingConstants.Id2,
                RouteId = QualificationRouteMappingConstants.RouteId2,
                Source = QualificationRouteMappingConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarsId = QualificationRouteMappingConstants.LarsId2,
                    Title = QualificationRouteMappingConstants.Title2,
                    ShortTitle = QualificationRouteMappingConstants.ShortTitle2
                }
            }
        };
    }
}
