﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;


namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute() : base(GetDefaultFixture)
        {

        }

        public static IFixture GetDefaultFixture()
        {
            var autoNSubstituteCustomization = new AutoNSubstituteCustomization
            {
                ConfigureMembers = true
            };

            return new Fixture()
                .Customize(new InMemoryDbContextCustomization())
                .Customize(new OpportunityCustomization())
                .Customize(new DomainCustomization())
                .Customize(new MapperCustomization())
                .Customize(autoNSubstituteCustomization);

        }
    }
}
