using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Results_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_Results_Is_Loaded()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" }
                }
                .AsQueryable();

            var logger = Substitute.For<ILogger<ProviderController>>();
            var mapper = Substitute.For<IMapper>();

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);
            var providerController = new ProviderController(logger, mapper, routePathService);
            providerController.CreateContextWithSubstituteTempData();

            _result = providerController.Results();
        }
        
        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();
    }
}
