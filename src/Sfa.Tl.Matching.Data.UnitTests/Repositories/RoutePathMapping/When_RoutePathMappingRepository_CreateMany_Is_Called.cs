using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping
{
    public class When_RoutePathMappingRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_RoutePathMappingRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<RoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidRoutePathMappingListBuilder().Build();

                var repository = new RoutePathMappingRepository(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}