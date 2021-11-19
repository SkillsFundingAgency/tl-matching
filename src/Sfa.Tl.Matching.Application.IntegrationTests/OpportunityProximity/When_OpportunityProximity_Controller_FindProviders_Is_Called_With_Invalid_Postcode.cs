using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly OpportunityProximityController _opportunityProximityController;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Invalid_Postcode()
        {
            const string requestPostcode = "cV12 34";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var routes = new List<SelectListItem>
            {
                new() { Text = "1", Value = "Route 1" },
                new() { Text = "2", Value = "Route 2" }
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = new LocationService(
                new LocationApiClient(httpClient, new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io"
                }));

            var opportunityProximityService = new OpportunityProximityService(Substitute.For<ISearchProvider>(), locationService);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRouteSelectListItemsAsync().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();

            _opportunityProximityController = new OpportunityProximityController(routePathService, opportunityProximityService, opportunityService, locationService);

            var selectedRouteId = routes.First().Text;
            const string postcode = requestPostcode;

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = int.Parse(selectedRouteId),
                Postcode = postcode
            };
            _result = _opportunityProximityController.FindProviders(viewModel).GetAwaiter().GetResult();
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
            _opportunityProximityController.ViewData.ModelState.IsValid.Should().BeFalse();
            _opportunityProximityController.ViewData.ModelState["Postcode"].Should().NotBeNull();
            _opportunityProximityController.ViewData.ModelState["Postcode"]?.Errors.Should().NotBeNull();
            _opportunityProximityController.ViewData.ModelState["Postcode"]?.Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}