using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders
{
    public class ValidQualificationRoutePathMappingBuilder
    {
        public Domain.Models.QualificationRoutePathMapping Build() => new Domain.Models.QualificationRoutePathMapping
        {
            Id = QualificationRoutePathMappingConstants.Id,
            PathId = QualificationRoutePathMappingConstants.PathId,
            Source = QualificationRoutePathMappingConstants.Source,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn,
            Qualification = new Qualification
            {
                LarsId = QualificationRoutePathMappingConstants.LarsId,
                Title = QualificationRoutePathMappingConstants.Title,
                ShortTitle = QualificationRoutePathMappingConstants.ShortTitle
            }
        };
    }
}
