﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class ReturnsRoutes
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private Task<IEnumerable<Route>> _result;
        private readonly IEnumerable<Route> _routeData
            = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
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

            _repository.GetRoutesAsync()
                .Returns(_routeData);

            _service = new RoutePathService(_repository);

            _result = _service.GetRoutesAsync();
        }

        [Test]
        public void ResultIsAsExpected()
        {
            Assert.AreEqual(_expected.First().Name, _result.Result.First().Name);
        }
    }
}
