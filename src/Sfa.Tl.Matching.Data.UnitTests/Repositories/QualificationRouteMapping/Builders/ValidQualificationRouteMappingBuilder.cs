using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders
{
    public class ValidQualificationRouteMappingBuilder
    {
        public Domain.Models.QualificationRouteMapping Build() => new Domain.Models.QualificationRouteMapping
        {
            Id = 1,
            RouteId = 2,
            Source = "Test",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn,
            Qualification = new Domain.Models.Qualification
            {
                LarsId = "1234567X",
                Title = "Full Qualification Title",
                ShortTitle = "Short Title"
            }
        };
    }
}
