using System;
using AutoFixture;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = 2;
            fixture.Customizations.Add(new NumericSequenceGenerator());

            fixture.Customize<MatchingConfiguration>(composer =>
                composer.With(config => config.SendEmailEnabled, true)
                    .With(config => config.EmployerFeedbackTimeSpan, "-10.00:00:00"));

            fixture.Customize<Provider>(composer => composer.With(p => p.IsCdfProvider, true)
                .With(p => p.IsEnabledForReferral, true));

            fixture.Customize<ProviderVenue>(composer => composer.With(pv => pv.IsRemoved, false)
                .With(pv => pv.IsEnabledForReferral, true));

            var crmId = Guid.NewGuid();
            fixture.Customize<Employer>(composer => composer.With(e => e.CrmId, crmId));

            var employer = fixture.Create<Employer>();
            fixture.Customize<Opportunity>(composer => composer.With(op => op.EmployerCrmId, employer.CrmId));

            fixture.Customize<OpportunityItem>(composer => composer.With(oi => oi.ModifiedOn, new DateTime(2019, 9, 1)));

        }
    }
}