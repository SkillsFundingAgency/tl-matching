using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetRoutes_Is_Called
    {
        private readonly IEnumerable<Route> _result;

        public When_RoutePathRepository_GetRoutes_Is_Called()
        {
            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Keyword",
                        Summary = "Route summary"
                    });
                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetRoutes().ToList();
            }
        }

        [Fact]
        public void Then_Route_Id_Is_Returned() => 
            _result.First().Id.Should().Be(1);

        [Fact]
        public void Then_Route_Name_Is_Returned() => 
            _result.First().Name.Should().BeEquivalentTo("Route 1");

        [Fact]
        public void Then_Route_Keywords_Is_Returned() => 
            _result.First().Keywords.Should().BeEquivalentTo("Keyword");

        [Fact]
        public void Then_Route_Summary_Id_Is_Returned() 
            => _result.First().Summary.Should().BeEquivalentTo("Route summary");
    }
}