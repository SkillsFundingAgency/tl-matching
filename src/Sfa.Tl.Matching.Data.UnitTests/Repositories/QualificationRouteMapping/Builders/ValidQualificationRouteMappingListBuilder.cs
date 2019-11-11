using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders
{
    public class ValidQualificationRouteMappingListBuilder
    {
        public IList<Domain.Models.QualificationRouteMapping> Build() => new List<Domain.Models.QualificationRouteMapping>
        {
            new Domain.Models.QualificationRouteMapping
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
            },
            new Domain.Models.QualificationRouteMapping
            {
                Id = 2,
                RouteId = 3,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarsId = "7654321X",
                    Title = "Another Qualification Title",
                    ShortTitle = "Another Short Title"
                }
            }
        };
    }
}
