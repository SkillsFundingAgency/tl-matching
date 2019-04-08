using System.Collections.Generic;
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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_RefineSearchResults_Is_Called
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_RefineSearchResults_Is_Called()
        {
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1"}
            }.AsQueryable();

            var logger = Substitute.For<ILogger<ProviderController>>();
            var mapper = Substitute.For<IMapper>();

            var providerService = Substitute.For<IProviderService>();
            providerService.IsValidPostCode(Arg.Any<string>()).Returns(true);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var providerController = new ProviderController(mapper, routePathService, providerService,
                opportunityService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV12WT",
                SelectedRouteId = 1,
                SearchRadius = 10
            };

            _result = providerController.RefineSearchResults(viewModel).GetAwaiter().GetResult();
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