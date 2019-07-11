using AutoFixture;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.AutoDomain
{
    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var opportunity = fixture.Create<Opportunity>();
            var item = fixture.Create<OpportunityItem>();

            fixture.Customize<OpportunityItem>(composer => 
                                                        composer.With(oi => oi.OpportunityId, opportunity.Id)
                                                                .With(oi => oi.Opportunity, opportunity)
                                                                .With(oi => oi.Id, item.Id));
            
            fixture.Customize<Referral>(composer => composer.With(referral => referral.OpportunityItemId, item.Id));
            fixture.Customize<ProvisionGap>(composer => composer.With(gap => gap.OpportunityItemId, item.Id));
            
            fixture.RepeatCount = 2;

        }
    }
}