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
    public class When_ProviderVenue_Add_Venue_Doesnt_Exist
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const int Id = 1;
        private const int ProviderId = 1;
        private const string Postcode = "CV1 2WT";
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_ProviderVenue_Add_Venue_Doesnt_Exist()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCodeAsync(Postcode).Returns((true, Postcode));
            _providerVenueService.GetVenue(ProviderId, Postcode).ReturnsNull();
            _providerVenueService.CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>()).Returns(Id);

            var providerVenueController = new ProviderVenueController(_providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            var viewModel = new AddProviderVenueViewModel
            {
                ProviderId = ProviderId,
                Postcode = Postcode
            };

            _result = controllerWithClaims.AddProviderVenue(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Redirect_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetProviderVenueDetail");
        }

        [Fact]
        public void Then_RouteValues_Has_VenueId()
        {
            var result = _result as RedirectToRouteResult;
            result?.RouteValues["providerVenueId"].Should().Be(Id);
        }

        [Fact]
        public void Then_IsValidPostCode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostCodeAsync(Postcode);
        }

        [Fact]
        public void Then_CreateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>());
        }
    }
}