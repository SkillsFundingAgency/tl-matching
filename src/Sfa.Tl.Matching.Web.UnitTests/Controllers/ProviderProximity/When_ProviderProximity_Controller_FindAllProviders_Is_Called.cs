using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_FindAllProviders_Is_Called
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_FindAllProviders_Is_Called()
        {
            const string postcode = "cv1 2wt";

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(postcode).Returns((true, postcode.ToUpper()));
            
            var routePathService = Substitute.For<IRoutePathService>();

            var providerProximityService = Substitute.For<IProviderProximityService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService, locationService);

            var viewModel = new ProviderProximitySearchParamViewModel
            {
                Postcode = postcode
            };

            _result = providerProximityController.FindAllProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderProximityResults");

            redirect?.RouteValues["searchCriteria"].Should().Be("CV1 2WT");
        }
    }
}