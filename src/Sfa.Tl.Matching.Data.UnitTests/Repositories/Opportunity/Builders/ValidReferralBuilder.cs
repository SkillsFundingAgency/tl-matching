using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidReferralBuilder
    {
        public Domain.Models.Referral Build() => new Domain.Models.Referral
        {
            Id = 1,
            OpportunityId = 1,
            ProviderVenueId = 1,
            DistanceFromEmployer = 3.5M,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
