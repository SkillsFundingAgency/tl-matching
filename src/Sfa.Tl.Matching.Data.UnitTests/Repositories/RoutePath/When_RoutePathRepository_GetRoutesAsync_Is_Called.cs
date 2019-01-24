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
                        Keywords = "Keyword",
                        Summary = "Route summary"
                    });
                dbContext.SaveChanges();

                var repository = new RoutePathRepository(dbContext);
                _result = repository.GetRoutesAsync();
            }
        }

        [Test]
        public async Task Then_Route_Id_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Id);
                });
        }

        [Test]
        public async Task Then_Route_Name_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Route 1", t.Result.First().Name);
                });
        }


        [Test]
        public async Task Then_Route_Keywords_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Keyword", t.Result.First().Keywords);
                });
        }
        
        [Test]
        public async Task Then_Route_Summary_Id_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Route summary", t.Result.First().Summary);
                });
        }
    }
}