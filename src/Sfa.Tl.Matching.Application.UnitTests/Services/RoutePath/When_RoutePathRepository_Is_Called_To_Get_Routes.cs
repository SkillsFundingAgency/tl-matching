using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathRepository_Is_Called_To_Get_Routes
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private Task<IEnumerable<Route>> _result;

        private readonly IEnumerable<Route> _routeData
            = new List<Route>
            {
                new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Keyword1",
                        Summary = "Route 1 summary",
                    },
                new Route
                {
                    Id = 2,
                    Name = "Route 2",
                    Keywords = "Keyword2",
                    Summary = "Route 2 summary",
                }
            };

        private readonly IEnumerable<Route> _expected
            = new List<Route>
            {
                new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Keyword1",
                        Summary = "Route 1 summary",
                    },
                new Route
                    { Id = 2,
                        Name = "Route 2",
                        Keywords = "Keyword2",
                        Summary = "Route 2 summary",
                    }
            };

        [SetUp]
        public void Setup()
        {
            _repository =
                Substitute
                    .For<IRoutePathRepository>();

            _repository.GetRoutesAsync()
                .Returns(_routeData);

            _service = new RoutePathService(_repository);

            _result = Task.FromResult(_service.GetRoutesAsync().Result);
        }

        [Test]
        public async Task Then_GetRoutesAsync__Is_Called_Exactly_Once()
        {
            await _repository
                .Received(1)
                .GetRoutesAsync();
        }

        [Test]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.AreEqual(_routeData.Count(), _result.Result.Count());
        }

        [Test]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Id, _result.Result.First().Id);
        }

        [Test]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Name, _result.Result.First().Name);
        }

        [Test]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Keywords, _result.Result.First().Keywords);
        }

        [Test]
        public void Then_Route_Summary_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Summary, _result.Result.First().Summary);
        }
    }
}
