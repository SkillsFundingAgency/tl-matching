using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_SearchPostcode_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_SearchPostcode_Is_Loaded()
        {
            var locationService = Substitute.For<ILocationService>();
            var providerProximityService = Substitute.For<IProviderProximityService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService, locationService);

            _result = providerProximityController.SearchPostcode();
        }

        [Fact]
        public void Then_ViewModel_Is_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().BeNull();
        }
    }
}