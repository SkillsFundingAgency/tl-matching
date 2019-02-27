using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Index_Post_Is_Called
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_Index_Post_Is_Called()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" },
                    new Route { Id = 2, Name = "Route 2" }
                }
                .AsQueryable();

            var logger = Substitute.For<ILogger<ProviderController>>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var providerService = Substitute.For<IProviderService>();

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var providerController = new ProviderController(logger, mapper, routePathService, providerService);
            providerController.CreateContextWithSubstituteTempData();

            var selectedRouteId = routes.First().Id.ToString();
            const int searchRadius = 5;
            const string postcode = "SW1A 2AA";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SearchRadius = searchRadius,
                SelectedRouteId = selectedRouteId,
                Postcode = postcode
            };
            _result = providerController.Index(viewModel);
        }
        
        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();
        
        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToActionResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToActionResult;
            redirect?.ActionName.Should().BeEquivalentTo("Results");
        }
    }
}
