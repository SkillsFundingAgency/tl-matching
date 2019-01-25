using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetRoutesAsync_Is_Called
    {
        private IEnumerable<Route> _result;

        [OneTimeSetUp]
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
                _result = await repository.GetRoutesAsync();
            }
        }

        [Test]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.AreEqual(_result.First().Id, _result.First().Id);
        }

        [Test]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.AreEqual("Route 1", _result.First().Name);
        }
        
        [Test]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.AreEqual("Keyword", _result.First().Keywords);
        }

        [Test]
        public void Then_Route_Summary_Id_Is_Returned()
        {
            Assert.AreEqual("Route summary", _result.First().Summary);
        }
    }
}