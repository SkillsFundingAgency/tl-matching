using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityItemListBuilder
    {
        public IList<OpportunityItem> Build() => new List<OpportunityItem>
            {
                new()
                {
                    Id = 1,
                    OpportunityId = 2,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy"
                },
                new()
                {
                    Id = 2,
                    OpportunityId = 2,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = true,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy"
                }
            };
    }
}
