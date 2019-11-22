using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Proximity
{
    public class When_Proximity_Controller_FindProviders_Is_Called_With_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly OpportunityProximityController _opportunityProximityController;

        public When_Proximity_Controller_FindProviders_Is_Called_With_Invalid_Postcode()
        {
            var routes = new List<Route>
            {
                new Route { Id = 1, Name = "Route 1" },
                new Route { Id = 2, Name = "Route 2" }
            }
            .AsQueryable();
            
            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var opportunityProximityService = new OpportunityProximityService(Substitute.For<ISearchProvider>(), 
                new LocationApiClient(new HttpClient(), new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" }),
                Substitute.For<IGoogleDistanceMatrixApiClient>());

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();

            _opportunityProximityController = new OpportunityProximityController(mapper, routePathService, opportunityProximityService, opportunityService, employerService);

            var selectedRouteId = routes.First().Id;
            const string postcode = "cV12 34";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = selectedRouteId,
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
            _opportunityProximityController.ViewData.ModelState["Postcode"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}