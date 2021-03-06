using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Detail_Is_Loaded()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenueWithQualificationsAsync(1)
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = 1,
                    Postcode = "CV1 2WT",
                    ProviderId = 1,
                    ProviderName = "ProviderName",
                    Source = "Admin",
                    Name = "VenueName"
                });

            var providerVenueController = new ProviderVenueController(_providerVenueService);

            _result = providerVenueController.GetProviderVenueDetailAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetVenueWithQualifications_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetVenueWithQualificationsAsync(1);
        }

        [Fact]
        public void Then_ViewModel_Values_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<ProviderVenueDetailViewModel>();
            viewModel.Id.Should().Be(1);
            viewModel.Postcode.Should().Be("CV1 2WT");
            viewModel.ProviderId.Should().Be(1);
            viewModel.ProviderName.Should().Be("ProviderName");
            viewModel.Source.Should().Be("Admin");
            viewModel.Name.Should().Be("VenueName");
        }
    }
}