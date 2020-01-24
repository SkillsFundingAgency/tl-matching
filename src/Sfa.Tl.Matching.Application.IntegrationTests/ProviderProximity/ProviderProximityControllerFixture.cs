using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderProximity
{
    public class ProviderProximityControllerFixture : LocationApiClientFixture
    {
        public ProviderProximityController ProviderProximityController;
        
        public void GetProviderProximityController(string requestPostcode)
        {
            GetLocationApiClient(requestPostcode);
            var locationService = new LocationService(LocationApiClient);
            var routePathService = Substitute.For<IRoutePathService>();
            var searchProvider = Substitute.For<ISearchProvider>();
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            var fileWriter = Substitute.For<IFileWriter<ProviderProximityReportDto>>();

            var providerProximityService = new ProviderProximityService(searchProvider, locationService, routePathService, fileWriter, datetimeProvider);

            var routes = new List<SelectListItem>
            {
                new SelectListItem { Text = "1", Value = "Route 1" },
                new SelectListItem { Text = "2", Value = "Route 2" }
            };

            var routeDictionary = new Dictionary<int, string>
            {
                { 1, "Route 1" },
                { 2, "Route 2" }
            };
            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);
            
            ProviderProximityController = new ProviderProximityController(routePathService, providerProximityService, locationService);
        }
    }
}