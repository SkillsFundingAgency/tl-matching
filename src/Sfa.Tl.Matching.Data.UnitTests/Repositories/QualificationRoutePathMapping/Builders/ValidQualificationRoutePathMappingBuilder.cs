using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders
{
    public class ValidQualificationRoutePathMappingBuilder
    {
        public Domain.Models.QualificationRoutePathMapping Build() => new Domain.Models.QualificationRoutePathMapping
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
        };
    }
}
