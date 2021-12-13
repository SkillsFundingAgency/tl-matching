using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders
{
    public class ValidProvisionGapBuilder
    {
        public Domain.Models.ProvisionGap Build() => new()
        {
            Id = 1,
            NoSuitableStudent = true,
            HadBadExperience = true,
            ProvidersTooFarAway = true,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
