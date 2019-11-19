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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Proximity
{
    public class When_Proximity_Controller_Index_Is_Loaded_With_Opportunity
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_Proximity_Controller_Index_Is_Loaded_With_Opportunity()
        {
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1"}
            }.AsQueryable();

            var mapper = Substitute.For<IMapper>();

            var proximityService = Substitute.For<IProximityService>();
            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService
                .GetCompanyNameWithAkaAsync(1)
                .Returns("CompanyName (AlsoKnownAs)");

            var employerService = Substitute.For<IEmployerService>();

            var proximityController = new ProximityController(mapper, routePathService, proximityService,
                _opportunityService, employerService);

            _result = proximityController.Index(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ViewModel_CompanyNameWithAka_Should_Have_Expected_Value()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SearchParametersViewModel>();
            viewModel.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");
        }
    }
}