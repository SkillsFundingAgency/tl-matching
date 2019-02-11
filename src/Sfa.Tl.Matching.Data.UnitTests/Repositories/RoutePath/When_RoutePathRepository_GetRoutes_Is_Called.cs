using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetRoutes_Is_Called
    {
        private IEnumerable<Route> _result;

        
        public async Task OneTimeSetup()
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
                _result = await repository.GetRoutes().ToListAsync();
            }
        }

        [Fact]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.Equal(_result.First().Id, _result.First().Id);
        }

        [Fact]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.Equal("Route 1", _result.First().Name);
        }
        
        [Fact]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.Equal("Keyword", _result.First().Keywords);
        }

        [Fact]
        public void Then_Route_Summary_Id_Is_Returned()
        {
            Assert.Equal("Route summary", _result.First().Summary);
        }
    }
}