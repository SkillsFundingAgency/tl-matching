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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called()
        {
            var routes = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
            }
            .AsQueryable();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, "CV1 2WT"));
            
            var opportunityProximityService = Substitute.For<IOpportunityProximityService>();
            
            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();

            var opportunityProximityController = new OpportunityProximityController(mapper, routePathService, opportunityProximityService, opportunityService,
                employerService, locationService);

            var selectedRouteId = routes.First().Id;
            const string postcode = "SW1A 2AA";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = selectedRouteId,
                Postcode = postcode
            };
            _result = opportunityProximityController.FindProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetOpportunityProviderResults");

            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["OpportunityId"].Should().Be(0);
            redirect?.RouteValues["OpportunityItemId"].Should().Be(0);
            redirect?.RouteValues["CompanyNameWithAka"].Should().BeNull();
        }
    }
}