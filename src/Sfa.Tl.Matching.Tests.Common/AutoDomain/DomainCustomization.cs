﻿using System;
using AutoFixture;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = 2;
            fixture.Customizations.Add(new NumericSequenceGenerator());

            fixture.Customize<MatchingConfiguration>(composer =>
                composer
                    .With(config => config.SendEmailEnabled, true)
                    .With(config => config.EmployerFeedbackWorkingDayInMonth, 10)
                    .With(config => config.ProviderFeedbackWorkingDayInMonth, 10)
                    .With(config => config.EmailDeliveryStatusToken, Guid.Parse("72b561ed-a7f3-4c0c-82a9-aae800a51de7"))
            );

            fixture.Customize<Provider>(composer =>
                composer.With(p => p.IsCdfProvider, true)
                    .With(p => p.IsEnabledForReferral, true)
                    .With(p => p.ProviderFeedbackSentOn, () => null)
            );

            fixture.Customize<ProviderVenue>(composer => composer.With(pv => pv.IsRemoved, false)
                .With(pv => pv.IsEnabledForReferral, true));

            var crmId = Guid.NewGuid();
            fixture.Customize<Employer>(composer => composer.With(e => e.CrmId, crmId));

            var employer = fixture.Create<Employer>();
            fixture.Customize<Opportunity>(composer => composer.With(op => op.EmployerCrmId, employer.CrmId));

            fixture.Customize<OpportunityItem>(composer => composer.With(oi => oi.ModifiedOn, new DateTime(2019, 9, 1)));

            fixture.Customize<EmailDeliveryStatusPayLoad>(composer => composer
                .With(payload => payload.Status, "delivered"));

        }
    }
}