using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_GetProviderDetail_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_GetProviderDetail_Is_Loaded()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService.GetProviderDetailByIdAsync(1).Returns(new ProviderDetailViewModel());

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());

            _result = providerController.GetProviderDetail(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_ProviderDetailViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Called_Exactly_Once()
        {
            _providerService.Received(1).GetProviderDetailByIdAsync(1);
        }
    }
}