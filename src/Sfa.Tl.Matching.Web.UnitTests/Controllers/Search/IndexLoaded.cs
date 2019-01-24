using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Search
{
    public class IndexLoaded
    {
        private IRoutePathService _routePathLookupService;
        private SearchController _controller;
        private IActionResult _result;

        [SetUp]
        //public async Task Setup()
        public void Setup()
        {
            _routePathLookupService =
                Substitute
                    .For<IRoutePathService>();

            var paths = Task.FromResult(
                new List<Path>
                {
                    new Path { Name = "Path 1" }
                }
                .AsEnumerable());
            var routes = Task.FromResult(
                new List<Route>
                    {
                    new Route { Name = "Route 1" }
                }
                .AsEnumerable());
            _routePathLookupService.GetPathsAsync().Returns(paths);
            _routePathLookupService.GetRoutesAsync().Returns(routes);

            _controller = new SearchController(_routePathLookupService);
            //var _result = await _controller.Index();
            //_result = Task.FromResult(_controller.Index()).Result.Result;
            _result = _controller.Index().Result;
        }

        [Test]
        public async Task GetRoutesAsyncIsCalledExactlyOnce()
        {
            await _routePathLookupService
                .Received(1)
                .GetRoutesAsync();
        }

        [Test]
        public void ResultContainsRoutes()
        {
            var view = _result as ViewResult;
            var model = view?.Model as IEnumerable<Route>;
            Assert.AreEqual("Route 1", model?.First().Name);
        }
    }
}