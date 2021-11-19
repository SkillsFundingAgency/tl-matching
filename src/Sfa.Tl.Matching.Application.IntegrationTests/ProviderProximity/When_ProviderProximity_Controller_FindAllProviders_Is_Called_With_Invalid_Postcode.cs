using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.TestClients;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
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

            var routes = new List<SelectListItem>
            {
                new() { Text = "1", Value = "Route 1" },
                new() { Text = "2", Value = "Route 2" }
            };

            var routeDictionary = new Dictionary<int, string>
            {
                { 1, "Route 1" },
                { 2, "Route 2" }
            };

            var locationService = new LocationService(
                new LocationApiClient(httpClient, new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io"
                }));

            var routePathService = Substitute.For<IRoutePathService>();
            var searchProvider = Substitute.For<ISearchProvider>();
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            var fileWriter = Substitute.For<IFileWriter<ProviderProximityReportDto>>();

            var providerProximityService = new ProviderProximityService(searchProvider, locationService, routePathService, fileWriter, datetimeProvider);

            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);

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