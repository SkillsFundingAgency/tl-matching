﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail.Builders
{
    public class ValidOpportunityItemListBuilder
    {
        public IList<OpportunityItem> Build() => new List<OpportunityItem>
        {
            new()
            {
                Id = 1,
                IsSaved = true,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.Referral.ToString()
            },
            new()
            {
                Id = 2,
                IsSaved = true,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.Referral.ToString()
            },
            new()
            {
                Id = 3,
                IsSaved = true,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.Referral.ToString()
            },
            new()
            {
                Id = 4,
                IsSaved = true,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.ProvisionGap.ToString()
            }
        };
    }
}
