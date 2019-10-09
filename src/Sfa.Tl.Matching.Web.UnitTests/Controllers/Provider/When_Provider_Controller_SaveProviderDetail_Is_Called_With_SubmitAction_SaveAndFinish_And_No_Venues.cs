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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish_And_No_Venues
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly ProviderController _controllerWithClaims;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish_And_No_Venues()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            _controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = _controllerWithClaims.SaveProviderDetailAsync(new ProviderDetailViewModel
            {
                Id = 1,
                SubmitAction = "SaveAndFinish"
            }).GetAwaiter().GetResult();
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
            ((ViewResult)_result).ViewName.Should().Be("ProviderDetail");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _providerService.DidNotReceive().UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }

        [Fact]
        public void Then_Model_State_Has_ProviderVenue_Error()
        {
            var modelStateEntry = _controllerWithClaims.ViewData.ModelState[nameof(ProviderVenue)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must add a venue for this provider");
        }
    }
}