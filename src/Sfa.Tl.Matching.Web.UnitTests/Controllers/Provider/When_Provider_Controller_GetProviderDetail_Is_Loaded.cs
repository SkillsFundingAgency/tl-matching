using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_GetProviderDetail_Is_Loaded : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        public When_Provider_Controller_GetProviderDetail_Is_Loaded()
        {
            _fixture = new ProviderControllerFixture();
            
            _fixture.ProviderService.GetProviderDetailByIdAsync(1)
                .Returns(new ProviderDetailViewModel
                {
                    Id = 1,
                    UkPrn = 123,
                    Name = "Provider Name",
                    DisplayName = "Provider Display Name",
                });

            _result = _fixture.Sut.GetProviderDetailAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Values_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            viewResult.Should().NotBeNull();

            viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();
            var viewModel = _result.GetViewModel<ProviderDetailViewModel>();
            viewModel.Id.Should().Be(1);
            
            viewModel.Name.Should().Be("Provider Name");
            viewModel.DisplayName.Should().Be("Provider Display Name");
            viewModel.UkPrn.Should().Be(123);
            viewModel.IsCdfProvider.Should().BeTrue();
            viewModel.IsEnabledForReferral.Should().BeTrue();
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Called_Exactly_Once()
        {
            _fixture.ProviderService.Received(1).GetProviderDetailByIdAsync(1);
        }
    }
}