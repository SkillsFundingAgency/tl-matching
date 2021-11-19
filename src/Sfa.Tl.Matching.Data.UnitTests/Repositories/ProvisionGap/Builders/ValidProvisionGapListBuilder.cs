using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders
{
    public class ValidProvisionGapListBuilder
    {
        public IList<Domain.Models.ProvisionGap> Build() => new List<Domain.Models.ProvisionGap>
        {
            new()
            {
                Id = 1,
                NoSuitableStudent = true,
                HadBadExperience = false,
                ProvidersTooFarAway = true,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                NoSuitableStudent = false,
                HadBadExperience = true,
                ProvidersTooFarAway = false,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
