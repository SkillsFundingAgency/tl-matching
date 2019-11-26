using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Add_Has_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        
        public When_ProviderVenue_Add_Has_Invalid_Postcode()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostcodeAsync("CV1 2WT").Returns((false, "CV1 2WT"));
            var providerVenueController = new ProviderVenueController(_providerVenueService);

            var viewModel = new AddProviderVenueViewModel
            {
                Postcode = "CV1 2WT"
            };

            _result = providerVenueController.CreateVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Contains_Error()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["Postcode"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }

        [Fact]
        public void Then_IsValidPostcode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostcodeAsync("CV1 2WT");
        }

        [Fact]
        public void Then_CreateVenue_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>());
        }
    }
}