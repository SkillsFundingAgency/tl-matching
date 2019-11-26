using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_GetProviderDetail_Is_Loaded_For_New_Provider
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_GetProviderDetail_Is_Loaded_For_New_Provider()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());

            _result = providerController.GetProviderDetailAsync(0).GetAwaiter().GetResult();
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
            _providerService.DidNotReceive().GetProviderDetailByIdAsync(Arg.Any<int>());
        }
    }
}