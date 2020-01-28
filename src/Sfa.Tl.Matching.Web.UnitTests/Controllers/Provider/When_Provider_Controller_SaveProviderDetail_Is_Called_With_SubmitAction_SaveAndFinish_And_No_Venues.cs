using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish_And_No_Venues : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;
        
        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish_And_No_Venues()
        {
            _fixture = new ProviderControllerFixture();
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SaveProviderDetailAsync(new ProviderDetailViewModel
            {
                Id = 1,
                SubmitAction = "SaveAndFinish"
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned_For_SearchProvider()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            ((ViewResult)_result).ViewName.Should().Be("ProviderDetail");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _fixture.ProviderService.DidNotReceive().UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }

        [Fact]
        public void Then_Model_State_Has_ProviderVenue_Error()
        {
            var modelStateEntry = _fixture.Sut.ViewData.ModelState[nameof(ProviderVenue)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must add a venue for this provider");
        }
    }
}