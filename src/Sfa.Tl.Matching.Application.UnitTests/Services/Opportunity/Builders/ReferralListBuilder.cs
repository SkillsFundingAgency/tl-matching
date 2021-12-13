using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class ReferralListBuilder
    {
        public IList<Domain.Models.Referral> Build() => new List<Domain.Models.Referral>
        {
            new()
            {
                ProviderVenue = new Domain.Models.ProviderVenue
                {
                    Postcode = "AA1 1AA",
                    Provider = new Domain.Models.Provider
                    {
                        Name = "Provider1"
                    }
                }
            }
        };
    }
}
