﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Proximity
{
    public class When_Proximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode
    {
        private readonly IActionResult _result;

        public When_Proximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode()
        {
            var routes = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
            }
            .AsQueryable();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var proximityService = Substitute.For<IProximityService>();
            proximityService.IsValidPostcode(Arg.Any<string>()).Returns((false, null));

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();

            var proximityController = new ProximityController(mapper, routePathService, 
                proximityService, opportunityService, employerService);

            var selectedRouteId = routes.First().Id;
            const int searchRadius = 5;
            const string postcode = "XYZ A12";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SearchRadius = searchRadius,
                SelectedRouteId = selectedRouteId,
                Postcode = postcode
            };
            _result = proximityController.FindProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchParametersViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<SearchParametersViewModel>();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["Postcode"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}