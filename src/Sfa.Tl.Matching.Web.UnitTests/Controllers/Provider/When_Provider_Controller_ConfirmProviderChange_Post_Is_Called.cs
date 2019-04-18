using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_ConfirmProviderChange_Post_Is_Called
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_ConfirmProviderChange_Post_Is_Called()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService);

            var viewModel = new HideProviderViewModel
            {
                ProviderId = 1,
                UkPrn = 10000546,
                ProviderName = "Test Provider"
            };
            _result = providerController.ConfirmProviderChange(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_SetIsProviderEnabledAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SetIsProviderEnabledForSearchAsync(Arg.Any<int>(), Arg.Any<bool>());
        }

        [Fact]
        public void Then_ProviderService_SetIsProviderEnabledAsync_Is_Called_With_Expected_ProviderId()
        {
            _providerService
                .Received(1)
                .SetIsProviderEnabledForSearchAsync(
                    Arg.Is<int>(p => p == 1),
                    Arg.Any<bool>());
        }

        [Fact]
        public void Then_ProviderService_SetIsProviderEnabledAsync_Is_Called_With_Expected_IsEnabled()
        {
            _providerService
                .Received(1)
                .SetIsProviderEnabledForSearchAsync(
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
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
        }

        [Fact]
        public void Then_Result_Is_Redirect_With_Id()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }
    }
}