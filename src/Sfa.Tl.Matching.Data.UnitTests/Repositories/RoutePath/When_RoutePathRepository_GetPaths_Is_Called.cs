using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePath
{
    public class When_RoutePathRepository_GetPaths_Is_Called
    {
        private IEnumerable<Path> _result;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
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
                _result = await repository.GetPaths().ToListAsync();
            }
        }

        [Test]
        public void Then_Path_Id_Is_Returned()
        {
            Assert.AreEqual(1, _result.First().Id);
        }

        [Test]
        public void Then_Path_RouteId_Is_Returned()
        {
            Assert.AreEqual(1, _result.First().RouteId);
        }

        [Test]
        public void Then_Path_Name_Is_Returned()
        {
            Assert.AreEqual("Path 1", _result.First().Name);
        }

        [Test]
        public void Then_Path_Keywords_Is_Returned()
        {
            Assert.AreEqual("Keyword", _result.First().Keywords);
        }

        [Test]
        public void Then_Path_Summary_Is_Returned()
        {
            Assert.AreEqual("Path summary", _result.First().Summary);
        }

        [Test]
        public void Then_Related_Route_Is_Returned()
        {
            Assert.IsNotNull(_result.First().Route.Id);
        }

        [Test]
        public void Then_Related_Route_Id_Is_Returned()
        {
            Assert.AreEqual(1, _result.First().Route.Id);
        }
    }
}