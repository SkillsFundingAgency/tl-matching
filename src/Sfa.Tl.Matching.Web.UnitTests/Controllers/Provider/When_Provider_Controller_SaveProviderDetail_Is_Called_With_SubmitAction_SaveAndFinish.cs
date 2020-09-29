using System.Collections.Generic;
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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndFinish()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.SaveProviderDetailAsync(new ProviderDetailViewModel
            {
                Id = 1,
                SubmitAction = "SaveAndFinish",
                ProviderVenues = new List<ProviderVenueViewModel>
                {
                    new ProviderVenueViewModel
                    {
                        Postcode = "CV1 2WT"
                    }
                }
            }).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_View_Result_Is_Returned_For_SearchProvider()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToActionResult>();

            var result = _result as RedirectToActionResult;
            result.Should().NotBeNull();
            result?.ActionName.Should().Be("SearchProvider");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _providerService.Received(1).UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }
    }
}