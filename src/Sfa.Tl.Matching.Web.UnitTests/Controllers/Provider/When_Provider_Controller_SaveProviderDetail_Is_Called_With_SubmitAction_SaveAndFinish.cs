using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService);

            _result = providerController.SaveProviderDetail(new ProviderDetailViewModel { Id = 1, SubmitAction = "SaveAndFinish" }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_View_Result_Is_Returned_For_SearchProvider()
        {
            _result.Should().BeAssignableTo<ViewResult>();
            ((ViewResult)_result).ViewName.Should().Be("SearchProvider");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _providerService.Received(1).UpdateProviderDetail(Arg.Any<ProviderDetailViewModel>());
        }
    }
}