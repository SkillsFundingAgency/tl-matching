using AutoFixture;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class SqlServerDbContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var dbcontext = new TestConfiguration().GetDbContext();

            fixture.Register(() => dbcontext);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}