using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Save_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Detail_Save_Submitted_Successfully()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();

            var providerVenueController = new ProviderVenueController(_providerVenueService);

            var viewModel = new ProviderVenueDetailViewModel
            {
                Id = 1,
                ProviderId = 2,
                Postcode = "CV1 2WT",
                Qualifications = new List<QualificationDetailViewModel>
                {
                    new()
                    {
                        LarId = "123"
                    }
                }
            };

            _result = providerVenueController.SaveProviderVenueDetailAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Get_Provider_Detail()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
            redirect?.RouteValues["providerId"].Should().Be(2);
        }

        [Fact]
        public void Then_GetVenueWithQualifications_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().GetVenueWithQualificationsAsync(1);
        }

        [Fact]
        public void Then_UpdateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).UpdateVenueAsync(Arg.Any<ProviderVenueDetailViewModel>());
        }
    }
}