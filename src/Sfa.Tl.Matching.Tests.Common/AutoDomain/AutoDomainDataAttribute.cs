using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;


namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute(bool useInMemoryDb = true) : base(() => GetDefaultFixture(useInMemoryDb))
        {
        }

        public static IFixture GetDefaultFixture(bool useInMemoryDb = true)
        {
            var autoNSubstituteCustomization = new AutoNSubstituteCustomization
            {
                ConfigureMembers = true
            };

            return new Fixture()
                .Customize(useInMemoryDb ? (ICustomization)(new InMemoryDbContextCustomization()) : new SqlServerDbContextCustomization())
                .Customize(new OpportunityCustomization())
                .Customize(new DomainCustomization())
                .Customize(new HttpContextCustomization())
                .Customize(new HttpContextAccessorCustomization())
                .Customize(new MapperCustomization())
                .Customize(new FactoryServiceCustomizations())
                .Customize(new EmailHistoryCustomization())
                .Customize(autoNSubstituteCustomization);
        }
    }
}
