using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_CreateMany_Is_Called : IClassFixture<RouteTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_RouteRepository_CreateMany_Is_Called(RouteTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreateRoute(3, "Route 3", createdBy: EntityCreationConstants.CreatedByUser)
                .CreateRoute(4, "Route 4", createdBy: EntityCreationConstants.CreatedByUser);

            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.Routes)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.Route.Count();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}