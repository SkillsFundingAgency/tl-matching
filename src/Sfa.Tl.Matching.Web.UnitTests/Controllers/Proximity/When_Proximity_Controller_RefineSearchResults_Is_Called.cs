﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Proximity
{
    public class When_Proximity_Controller_RefineSearchResults_Is_Called
    {
        private readonly IActionResult _result;

        public When_Proximity_Controller_RefineSearchResults_Is_Called()
        {
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1"}
            }.AsQueryable();

            var logger = Substitute.For<ILogger<ProviderController>>();
            var mapper = Substitute.For<IMapper>();

            var proximityService = Substitute.For<IProximityService>();
            proximityService.IsValidPostCode(Arg.Any<string>()).Returns((true, "CV1 2WT"));

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var proximityController = new ProximityController(mapper, routePathService, proximityService, opportunityService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV12WT",
                SelectedRouteId = 1,
                SearchRadius = 10
            };

            _result = proximityController.RefineSearchResults(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<SearchViewModel>();
        }
    }
}