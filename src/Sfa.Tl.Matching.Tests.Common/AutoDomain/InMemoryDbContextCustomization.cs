using System;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class InMemoryDbContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var options = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging()
                .Options;

            var dbcontext = new MatchingDbContext(options);
            fixture.Register(() => dbcontext);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}