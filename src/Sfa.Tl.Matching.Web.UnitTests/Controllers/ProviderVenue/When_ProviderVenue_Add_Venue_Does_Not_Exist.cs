using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Add_Venue_Does_Not_Exist
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Add_Venue_Does_Not_Exist()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostcodeAsync("CV1 2WT").Returns((true, "CV1 2WT"));
            _providerVenueService.GetVenueAsync(1, "CV1 2WT").ReturnsNull();
            _providerVenueService.CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>()).Returns(1);

            var providerVenueController = new ProviderVenueController(_providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new AddProviderVenueViewModel
            {
                ProviderId = 1,
                Postcode = "CV1 2WT"
            };

            _result = controllerWithClaims.CreateVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();

            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetProviderVenueDetail");
            result?.RouteValues["providerVenueId"].Should().Be(1);
        }

        [Fact]
        public void Then_IsValidPostcode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostcodeAsync("CV1 2WT");
        }

        [Fact]
        public void Then_CreateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>());
        }
    }
}