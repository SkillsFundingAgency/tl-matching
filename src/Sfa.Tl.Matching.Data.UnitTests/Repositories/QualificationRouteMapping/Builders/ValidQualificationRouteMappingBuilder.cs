using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders
{
    public class ValidQualificationRouteMappingBuilder
    {
        public Domain.Models.QualificationRouteMapping Build() => new Domain.Models.QualificationRouteMapping
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
        };
    }
}
