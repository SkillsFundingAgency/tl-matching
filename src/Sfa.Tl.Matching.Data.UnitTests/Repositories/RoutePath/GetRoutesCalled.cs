using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class GetRoutesCalled
    {
        private Task<IEnumerable<Route>> _result;

        [SetUp]
        public void Setup()
        {
            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Word",
                        Summary = "Route summary"
                    });
                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetRoutesAsync();
            }
        }

        [Test]
        public async Task GetRoutesAsyncReturnsExpectedRoutes()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Id);
                    Assert.AreEqual("Route 1", t.Result.First().Name);
                    Assert.AreEqual("Word", t.Result.First().Keywords);
                    Assert.AreEqual("Route summary", t.Result.First().Summary);
                });
        }
    }
}