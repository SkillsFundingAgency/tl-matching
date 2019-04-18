using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Controller_ConfirmProviderVenueChange_Post_Is_Called
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Controller_ConfirmProviderVenueChange_Post_Is_Called()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper, _providerVenueService);

            var viewModel = new HideProviderVenueViewModel
            {
                ProviderVenueId = 1,
                ProviderName = "Test Provider"
            };
            _result = providerVenueController.ConfirmProviderVenueChange(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueService_SetIsProviderVenueEnabledForSearchAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .SetIsProviderVenueEnabledForSearchAsync(Arg.Any<int>(), Arg.Any<bool>());
        }

        [Fact]
        public void
            Then_ProviderVenueService_SetIsProviderVenueEnabledForSearchAsync_Is_Called_With_Expected_ProviderVenueId()
        {
            _providerVenueService
                .Received(1)
                .SetIsProviderVenueEnabledForSearchAsync(
                    Arg.Is<int>(p => p == 1),
                    Arg.Any<bool>());
        }

        [Fact]
        public void
            Then_ProviderVenueService_SetIsProviderVenueEnabledForSearchAsync_Is_Called_With_Expected_IsEnabled()
        {
            _providerVenueService
                .Received(1)
                .SetIsProviderVenueEnabledForSearchAsync(
                    Arg.Any<int>(),
                    Arg.Is<bool>(s => s));
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Provider_Detail()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderVenueDetail");
        }

        [Fact]
        public void Then_Result_Is_Redirect_With_Id()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("id", 1));
        }
    }
}