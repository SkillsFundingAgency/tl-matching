﻿using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.TestClients;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Unformatted_Postcode
    {
        private readonly IActionResult _result;
        private readonly IRoutePathService _routeService;

        public When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Unformatted_Postcode()
        {
            const string requestPostcode = "Cv 12 Wt";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var locationService = new LocationService(new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io"
            }));

            var opportunityProximityService = new OpportunityProximityService(Substitute.For<ISearchProvider>(), locationService);

            _routeService = Substitute.For<IRoutePathService>();
            _routeService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(_routeService, opportunityProximityService, opportunityService, locationService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = requestPostcode,
                SelectedRouteId = 1
            };

            _result = opportunityProximityController.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteValues.Count.Should().BeGreaterOrEqualTo(3);
            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var result = _result as RedirectToRouteResult;
            // ReSharper disable once PossibleNullReferenceException
            result.RouteValues.Count.Should().BeGreaterOrEqualTo(3);
        }

        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _routeService.Received(1).GetRouteIdsAsync();
        }
    }
}