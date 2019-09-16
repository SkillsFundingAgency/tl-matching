using System.Collections.Generic;
using System.Net;
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
                    .With(config => config.NotificationsSystemId, "TLevelsIndustryPlacement"));

            fixture.Customize<Provider>(composer => composer.With(p => p.IsCdfProvider, true)
                .With(p => p.IsEnabledForReferral, true));

            fixture.Customize<ProviderVenue>(composer => composer.With(pv => pv.IsRemoved, false)
                .With(pv => pv.IsEnabledForReferral, true));
        }
    }
}