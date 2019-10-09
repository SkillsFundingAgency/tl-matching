using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Controller_ConfirmRemoveProviderVenue_Post_Is_Called
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Controller_ConfirmRemoveProviderVenue_Post_Is_Called()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();

            var providerVenueController = new ProviderVenueController(_providerVenueService);

            var viewModel = new RemoveProviderVenueViewModel
            {
                ProviderId = 1,
                ProviderVenueId = 1
            };
            _result = providerVenueController.ConfirmRemoveProviderVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }
        
        [Fact]
        public void
            Then_ProviderVenueService_UpdateVenueAsync_Is_Called_With_Expected_Values()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueAsync(
                    Arg.Is<RemoveProviderVenueViewModel>(
                        vm => vm.ProviderVenueId == 1));
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Provider_Detail_With_Provider_Id()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }
    }
}