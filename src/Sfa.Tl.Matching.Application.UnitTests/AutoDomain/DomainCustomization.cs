using AutoFixture;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.AutoDomain
{
    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Referral>(composer => composer.Without(referral => referral.OpportunityItem));
            fixture.Customize<ProvisionGap>(composer => composer.Without(gap => gap.OpportunityItem));

            fixture.PostprocessorFor<OpportunityItem>(item =>
            {
                foreach (var referral in item.Referral)
                {
                    referral.OpportunityItem = item;
                }
            });

            fixture.PostprocessorFor<OpportunityItem>(item =>
            {
                foreach (var gap in item.ProvisionGap)
                {
                    gap.OpportunityItem = item;
                }
            });
        }
    }
}