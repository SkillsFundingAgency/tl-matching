using System.Collections.Generic;
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
            var routes = new List<Domain.Models.Route>
            {
                new Domain.Models.Route
                {
                    Id = 991,
                    Name = "Route 991",
                    CreatedBy =  EntityCreationConstants.CreatedByUser
                },
                new Domain.Models.Route
                {
                    Id = 992,
                    Name = "Route 992",
                    CreatedBy =  EntityCreationConstants.CreatedByUser
                }
            };

            testFixture.Builder.ClearData();

            _result = testFixture.Repository.CreateManyAsync(routes)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.Route.Count();

            foreach (var route in routes)
            {
                testFixture.Builder.Routes.Add(route);
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}