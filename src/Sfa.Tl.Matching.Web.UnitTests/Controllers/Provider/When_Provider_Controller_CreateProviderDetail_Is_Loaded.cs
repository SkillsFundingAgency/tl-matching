using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_CreateProviderDetail_Is_Loaded : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_CreateProviderDetail_Is_Loaded()
        {
            var fixture = new ProviderControllerFixture();
            var controllerWithClaims = fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.AddProviderDetail(new AddProviderViewModel
            {
                UkPrn = 123,
                Name = "Provider name"
            });
        }

        [Fact]
        public void Then_View_Model_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();

            var viewModel = _result.GetViewModel<ProviderDetailViewModel>();
            viewModel.Name.Should().Be("Provider name");
            viewModel.DisplayName.Should().Be("Provider Name");
            viewModel.UkPrn.Should().Be(123);
            viewModel.IsCdfProvider.Should().BeTrue();
            viewModel.IsEnabledForReferral.Should().BeTrue();
        }
    }
}