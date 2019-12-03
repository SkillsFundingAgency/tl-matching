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
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Unformatted_Postcode
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Unformatted_Postcode()
        {
            const string requestPostcode = "cV12 Wt";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var routes = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
            }
            .AsQueryable();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = new LocationService(
                new LocationApiClient(httpClient, new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io"
                }));
            
            var opportunityProximityService = new OpportunityProximityService(Substitute.For<ISearchProvider>(), 
                locationService);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(routePathService, opportunityProximityService, opportunityService, 
                locationService);

            var selectedRouteId = routes.First().Id;
            const string postcode = requestPostcode;

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

            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
        }
    }
}