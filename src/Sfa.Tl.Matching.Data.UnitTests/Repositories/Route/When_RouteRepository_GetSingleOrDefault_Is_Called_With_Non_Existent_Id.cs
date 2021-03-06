using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.Route _result;

        public When_RouteRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Route>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidRouteBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Route>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Route_Is_Returned() =>
            _result.Should().BeNull();
    }
}