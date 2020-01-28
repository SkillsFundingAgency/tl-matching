using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_GetProviderDetail_Is_Loaded_For_New_Provider : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        public When_Provider_Controller_GetProviderDetail_Is_Loaded_For_New_Provider()
        {
            _fixture = new ProviderControllerFixture();

            _result = _fixture.Sut.GetProviderDetailAsync(0).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Provider_Id_Is_Not_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();

            var viewModel = _result.GetViewModel<ProviderDetailViewModel>();
            viewModel.Id.Should().Be(0);
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Not_Called()
        {
            _fixture.ProviderService.DidNotReceive().GetProviderDetailByIdAsync(Arg.Any<int>());
        }
    }
}