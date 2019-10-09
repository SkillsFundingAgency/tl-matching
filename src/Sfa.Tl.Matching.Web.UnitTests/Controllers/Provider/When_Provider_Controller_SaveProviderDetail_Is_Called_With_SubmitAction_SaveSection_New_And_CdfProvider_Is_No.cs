using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly ProviderDetailViewModel _viewModel = new ProviderDetailViewModel
        {
            SubmitAction = "SaveSection",
            IsCdfProvider = false
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.SaveProviderDetailAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().Be("SearchProvider");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailSectionAsync_Is_Not_Called()
        {
            _providerService.DidNotReceive().UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}