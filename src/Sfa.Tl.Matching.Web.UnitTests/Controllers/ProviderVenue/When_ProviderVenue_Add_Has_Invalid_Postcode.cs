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

            _result = providerVenueController.AddProviderVenue(viewModel).GetAwaiter().GetResult();
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