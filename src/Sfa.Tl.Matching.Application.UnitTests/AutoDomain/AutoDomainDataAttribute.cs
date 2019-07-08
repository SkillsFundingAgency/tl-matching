using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Sfa.Tl.Matching.Application.UnitTests.AutoDomain
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
                .Customize(autoNSubstituteCustomization);

        }
    }
}
