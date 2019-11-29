﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.TestClients;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderProximity
{
    public class When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly ProviderProximityController _providerProximityController;

        public When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Invalid_Postcode()
        {
            const string requestPostcode = "cV12 34";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var routes = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
            }
            .AsQueryable();
            
            var locationService = new LocationService(
                new LocationApiClient(httpClient, new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io"
                }));

            var searchProvider = Substitute.For<ISearchProvider>();
            var cacheService = Substitute.For<ICacheService>();

            var providerProximityService = new ProviderProximityService(searchProvider,
                locationService, cacheService);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);
            
            _providerProximityController = new ProviderProximityController(routePathService, providerProximityService, locationService);

            const string postcode = requestPostcode;

            var viewModel = new ProviderProximitySearchParamViewModel
            {
                Postcode = postcode
            };
            _result = _providerProximityController.FindAllProviders(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            _providerProximityController.ViewData.ModelState.IsValid.Should().BeFalse();
            _providerProximityController.ViewData.ModelState["Postcode"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}