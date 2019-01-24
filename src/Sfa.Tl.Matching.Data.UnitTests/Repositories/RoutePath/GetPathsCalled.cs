using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class GetPathsCalled
    {
        private Task<IEnumerable<Path>> _result;

        [SetUp]
        public void Setup()
        {
            using (var dbContext = InMemoryDbContext.Create())
            {
                var route = new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Word",
                        Summary = "Route summary"
                    };
                dbContext.Add(route);

                dbContext.Add(
                    new Path
                    {
                        Id = 1,
                        RouteId = 1,
                        Name = "Path 1",
                        Keywords = "Word",
                        Summary = "Path summary",
                        Route = route
                    });

                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetPathsAsync();
            }
        }
       
        [Test]
        public async Task GetPathsAsyncReturnsExpectedPaths()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Id);
                    Assert.AreEqual(1, t.Result.First().RouteId);
                    Assert.AreEqual("Path 1", t.Result.First().Name);
                    Assert.AreEqual("Word", t.Result.First().Keywords);
                    Assert.AreEqual("Path summary", t.Result.First().Summary);

                    Assert.IsNotNull(t.Result.First().Route.Id);
                    Assert.AreEqual(1, t.Result.First().Route.Id);

                });
        }
    }
}