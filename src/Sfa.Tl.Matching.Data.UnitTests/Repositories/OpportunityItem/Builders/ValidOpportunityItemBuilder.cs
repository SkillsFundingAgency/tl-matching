using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem.Builders
{
    public class ValidOpportunityItemBuilder
    {
        public Domain.Models.OpportunityItem Build() => new()
        {
            Id = 1,
            RouteId = 1,
            OpportunityType = OpportunityType.Referral.ToString(),
            Postcode = "AA1 1AA",
            SearchRadius = 10,
            JobRole = "Testing Job Title",
            PlacementsKnown = true,
            Placements = 3,
            SearchResultProviderCount = 12,
            IsSaved = true,
            IsSelectedForReferral = true,
            IsCompleted = true,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
