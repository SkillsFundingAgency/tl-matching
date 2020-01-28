using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;
        private readonly ProviderDetailViewModel _viewModel = new ProviderDetailViewModel
        {
            SubmitAction = "SaveSection",
            IsCdfProvider = false
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No()
        {
            _fixture = new ProviderControllerFixture();

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SaveProviderDetailAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider()
        {
            _result.Should().NotBeNull();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().Be("SearchProvider");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailSectionAsync_Is_Not_Called()
        {
            _fixture.ProviderService.DidNotReceive().UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}