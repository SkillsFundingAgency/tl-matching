using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class RepositoryCalledForGetRoutes
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private readonly IEnumerable<Route> _routeData
            = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
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

            _service.GetRoutesAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task GetRoutesAsyncIsCalledExactlyOnce()
        {
            await _repository
                .Received(1)
                .GetRoutesAsync();
        }
    }
}
