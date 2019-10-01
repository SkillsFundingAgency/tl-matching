using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_RouteRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Route>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidRouteListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.Route>(logger, dbContext);
                _result = repository.CreateManyAsync(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}