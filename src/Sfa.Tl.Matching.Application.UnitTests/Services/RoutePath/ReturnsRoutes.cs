using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Core.DomainModels;
using Sfa.Tl.Matching.Core.Interfaces;
using Sfa.Tl.Matching.Data.Models;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class ReturnsRoutes
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private Task<IEnumerable<Route>> _result;
        private readonly IEnumerable<RoutePathLookup> _routePathData
            = new List<RoutePathLookup>
            {
                new RoutePathLookup { Id = 1, Route = "Route 1", Path = "Path 1"},
                new RoutePathLookup { Id = 2, Route = "Route 2", Path = "Path 2"}
            };
        private readonly IEnumerable<Route> _expected
            = new List<Route>
            {
                new Route { Name = "Route 1"},
                new Route { Name = "Route 2"}
            };

        [SetUp]
        public void Setup()
        {
            _repository =
                Substitute
                    .For<IRoutePathRepository>();

            _repository.GetListAsync()
                .Returns(_routePathData);

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<RoutePathLookupMappingProfile>());
            var mapper = mapConfig.CreateMapper();

            _service = new RoutePathService(_repository, mapper);

            _result = _service.GetRoutesAsync();
        }

        [Test]
        public void ResultIsAsExpected()
        {
            Assert.AreEqual(_expected.First().Name, _result.Result.First().Name);
        }
    }
}
