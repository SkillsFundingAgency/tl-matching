using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders
{
    public class ValidProviderQualificationListBuilder
    {
        public IList<Domain.Models.ProviderQualification> Build() => new List<Domain.Models.ProviderQualification>
        { 
            new()
            {
                Id = 1,
                ProviderVenueId = 1,
                QualificationId = 1,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                ProviderVenueId = 2,
                QualificationId = 2,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
