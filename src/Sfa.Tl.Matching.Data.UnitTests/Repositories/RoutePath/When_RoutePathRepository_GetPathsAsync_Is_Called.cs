using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetPathsAsync_Is_Called
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
                    Keywords = "Keyword",
                    Summary = "Route summary"
                };
                dbContext.Add(route);

                dbContext.Add(
                    new Path
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
                _result = repository.GetPathsAsync();
            }
        }

        [Test]
        public async Task Then_Path_Id_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Id);
                });
        }

        [Test]
        public async Task Then_Path_RouteId_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().RouteId);
                });
        }

        [Test]
        public async Task Then_Path_Name_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Path 1", t.Result.First().Name);
                });
        }

        [Test]
        public async Task Then_Path_Keywords_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Keyword", t.Result.First().Keywords);
                });
        }

        [Test]
        public async Task Then_Path_Summary_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual("Path summary", t.Result.First().Summary);
                });
        }

        [Test]
        public async Task Then_Related_Route_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.IsNotNull(t.Result.First().Route.Id);
                });
        }

        [Test]
        public async Task Then_Related_Route_Id_Is_Returned()
        {
            await _result.ContinueWith(
                t =>
                {
                    Assert.AreEqual(1, t.Result.First().Route.Id);
                });
        }
    }
}