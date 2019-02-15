using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.Data.RoutePathMapping.Builders;
using Sfa.Tl.Matching.Data.Repositories;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping
{
    public class When_RoutePathMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.RoutePathMapping _result;

        public When_RoutePathMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<RoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidRoutePathMappingBuilder().Build());
                dbContext.SaveChanges();

                var repository = new RoutePathMappingRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_RoutePathMapping_Is_Returned() =>
            _result.Should().BeNull();
    }
}