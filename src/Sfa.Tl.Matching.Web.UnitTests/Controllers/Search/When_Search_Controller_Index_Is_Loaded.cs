using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Search
{
    public class When_Search_Controller_Index_Is_Loaded
    {
        private IRoutePathService _routePathLookupService;
        private SearchController _controller;
        private IActionResult _result;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _routePathLookupService =
                Substitute
                    .For<IRoutePathService>();

            var paths = new List<Path>
                {
                    new Path { Name = "Path 1" }
                }
                .AsQueryable();

            var routes = new List<Route>
                {
                    new Route { Name = "Route 1" }
                }
                .AsQueryable();

            _routePathLookupService.GetPaths().Returns(paths);
            _routePathLookupService.GetRoutes().Returns(routes);

            _controller = new SearchController(_routePathLookupService);
            _result = _controller.Index();
        }

        [Test]
        public void Then_GetRoutes_Is_Called_Exactly_Once()
        {
            _routePathLookupService
                .Received(1)
                .GetRoutes();
        }

        [Test]
        public void Then_Result_Contains_Routes()
        {
            var view = _result as ViewResult;
            var model = view?.Model as IEnumerable<Route>;
            Assert.AreEqual("Route 1", model?.First().Name);
        }
    }
}