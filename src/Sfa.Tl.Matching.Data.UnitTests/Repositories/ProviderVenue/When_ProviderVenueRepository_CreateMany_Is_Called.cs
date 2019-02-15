using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue
{
    public class When_ProviderVenueRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_ProviderVenueRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderVenueRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidProviderVenueListBuilder().Build();

                var repository = new ProviderVenueRepository(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}