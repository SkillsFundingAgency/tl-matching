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
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_RefineSearchResults_Is_Called
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_RefineSearchResults_Is_Called()
        {
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1"}
            }.AsQueryable();

            var mapper = Substitute.For<IMapper>();

            var proximityService = Substitute.For<IProximityService>();
            proximityService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, "CV1 2WT"));

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();

            var opportunityProximityController = new OpportunityProximityController(mapper, routePathService, proximityService, opportunityService,
                employerService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV12WT",
                SelectedRouteId = 1,
                CompanyNameWithAka = "CompanyName (AlsoKnownAs)"
            };

            _result = opportunityProximityController.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderResults");

            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["OpportunityId"].Should().Be(0);
            redirect?.RouteValues["OpportunityItemId"].Should().Be(0);
            redirect?.RouteValues["CompanyNameWithAka"].Should().Be("CompanyName (AlsoKnownAs)");

        }
    }
}