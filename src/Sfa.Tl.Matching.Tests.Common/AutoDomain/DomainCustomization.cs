﻿using System.Collections.Generic;
using AutoFixture;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class DomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = 2;
            fixture.Customizations.Add(new NumericSequenceGenerator());

            var opportunity = fixture.Create<Opportunity>();
            const int itemId = 100;

            fixture.Customize<OpportunityItem>(composer => 
                                                        composer.With(oi => oi.OpportunityId, opportunity.Id)
                                                                .With(oi => oi.Opportunity, opportunity)
                                                                .With(oi => oi.Id, itemId));

            fixture.Customize<Referral>(composer => composer.With(referral => referral.OpportunityItemId, itemId));
            fixture.Customize<ProvisionGap>(composer => composer.With(gap => gap.OpportunityItemId, itemId));

            var items = fixture.Create<List<OpportunityItem>>();

            fixture.Customize<Opportunity>(composer => composer.With(op => op.Id, opportunity.Id)
                .With(op => op.OpportunityItem, items ));


        }
    }
}