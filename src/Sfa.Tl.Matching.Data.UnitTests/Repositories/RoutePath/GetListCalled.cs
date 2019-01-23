using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Data.Models;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class GetListCalled
    {
        private Task<IEnumerable<RoutePathLookup>> _result;

        [SetUp]
        public void Setup()
        {
            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new RoutePathLookup
                    {
                        Id = 1,
                        Route = "Route 1",
                        Path = "Path 1"
                    });
                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetListAsync();
            }
        }

        [Test]
        public async Task GetListAsyncCallsRepository()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Id);
                    //Assert.AreEqual("Route 2", t.Result.First().Route, "Route failed");
                    //Assert.AreEqual("Path 1", t.Result.First().Path);
                });
        }
    }
}