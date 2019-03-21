using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidProviderQualificationListBuilder
    {
        public IList<Domain.Models.ProviderQualification> Build() => new List<Domain.Models.ProviderQualification>
        {
            new Domain.Models.ProviderQualification
            {
                Id = 1,
                ProviderVenueId = 1,
                QualificationId = 1,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.ProviderQualification
            {
                Id = 2,
                ProviderVenueId = 1,
                QualificationId = 2,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.ProviderQualification
            {
                Id = 3,
                ProviderVenueId = 1,
                QualificationId = 3,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
