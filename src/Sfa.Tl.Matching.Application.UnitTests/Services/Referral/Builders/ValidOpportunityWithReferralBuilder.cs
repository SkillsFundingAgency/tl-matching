using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidOpportunityWithReferralBuilder
    {
        public Domain.Models.Opportunity Build() => new Domain.Models.Opportunity
        {
            Id = 1,
            PostCode = "AA1 1AA",
            Distance = 5,
            RouteId = 2,
            Referral = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral
                {
                    Id = 1,
                    OpportunityId = 1,
                }
            },
            Route = new Route
            {
                Id = 1,
                Name = "Agriculture, environmental and animal care"
            }
        };
    }
}
