using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.Repositories;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetPaths_Is_Called
    {
        private readonly IEnumerable<Domain.Models.Path> _result;

        public When_RoutePathRepository_GetPaths_Is_Called()
        {
            using (var dbContext = InMemoryDbContext.Create())
            {
                var route = new Domain.Models.Route
                {
                    Id = 1,
                    Name = "Route 1",
                    Keywords = "Keyword",
                    Summary = "Route summary"
                };
                dbContext.Add(route);

                dbContext.Add(
                    new Domain.Models.Path
                    {
                        Id = 1,
                        RouteId = 1,
                        Name = "Path 1",
                        Keywords = "Keyword",
                        Summary = "Path summary",
                        Route = route
                    });

                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetPaths().ToList();
            }
        }

        [Fact]
        public void Then_Path_Id_Is_Returned() =>
            _result.First().Id.Should().Be(1);

        [Fact]
        public void Then_Path_RouteId_Is_Returned() =>
            _result.First().RouteId.Should().Be(1);

        [Fact]
        public void Then_Path_Name_Is_Returned() =>
            _result.First().Name.Should().BeEquivalentTo("Path 1");

        [Fact]
        public void Then_Path_Keywords_Is_Returned() =>
            _result.First().Keywords.Should().BeEquivalentTo("Keyword");

        [Fact]
        public void Then_Path_Summary_Is_Returned() =>
            _result.First().Summary.Should().BeEquivalentTo("Path summary");

        [Fact]
        public void Then_Related_Route_Is_Returned() =>
            _result.First().Route.Id.Should().Be(1);

        [Fact]
        public void Then_Related_Route_Id_Is_Returned() =>
            _result.First().Route.Id.Should().Be(1);
    }
}