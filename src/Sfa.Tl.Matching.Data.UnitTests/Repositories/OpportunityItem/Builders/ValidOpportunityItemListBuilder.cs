using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem.Builders
{
    public class ValidOpportunityItemListBuilder
    {
        public IList<Domain.Models.OpportunityItem> Build() => new List<Domain.Models.OpportunityItem>
        {
            new Domain.Models.OpportunityItem
            {
                Id = 1,
                RouteId = 1,
                OpportunityType = OpportunityType.Referral.ToString(),
                Postcode = "AA1 1AA",
                SearchRadius = 10,
                JobTitle = "Testing Job Title",
                PlacementsKnown = true,
                Placements = 3,
                SearchResultProviderCount = 12,
                IsSaved = true,
                IsSelectedForReferral  = true,
                IsCompleted  = true,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.OpportunityItem
            {
                Id = 2,
                RouteId = 2,
                OpportunityType = OpportunityType.ProvisionGap.ToString(),
                Postcode = "AA1 1AA",
                SearchRadius = 10,
                JobTitle = "Testing Job Title",
                PlacementsKnown = true,
                Placements = 3,
                SearchResultProviderCount = 12,
                IsSaved = true,
                IsSelectedForReferral  = true,
                IsCompleted  = true,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
