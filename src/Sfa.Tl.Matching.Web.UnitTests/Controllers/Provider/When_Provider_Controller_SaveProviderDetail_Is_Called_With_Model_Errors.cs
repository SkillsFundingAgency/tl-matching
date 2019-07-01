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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_No_Venues
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly ProviderController _providerController;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_No_Venues()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerController = new ProviderController(_providerService, new MatchingConfiguration());

            _result = _providerController.SaveProviderDetail(new ProviderDetailViewModel()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_ProviderService_Is_Not_Called() =>
            _providerService.DidNotReceiveWithAnyArgs();

        [Fact]
        public void Then_ModelState_Is_Not_Valid()
        {
            _providerController.ViewData.ModelState.IsValid.Should().BeFalse();
            _providerController.ViewData.ModelState.Count.Should().Be(1);
            _providerController.ViewData.ModelState["ProviderVenue"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must add a venue for this provider");
        }
    }
}