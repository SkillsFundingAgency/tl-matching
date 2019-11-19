using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Controller_ConfirmRemoveProviderVenue_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Controller_ConfirmRemoveProviderVenue_Is_Loaded()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetRemoveProviderVenueViewModelAsync(Arg.Any<int>())
                .Returns(new ValidRemoveProviderVenueViewModelBuilder().Build());

            var providerVenueController = new ProviderVenueController(_providerVenueService);

            _result = providerVenueController.GetConfirmRemoveProviderVenueAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueService_GetRemoveProviderVenueViewModelAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetRemoveProviderVenueViewModelAsync(1);
        }

        [Fact]
        public void Then_View_Model_Fields_Have_Expected_Value()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<RemoveProviderVenueViewModel>();

            var model = viewResult?.Model as RemoveProviderVenueViewModel;
            model.Should().NotBeNull();
            model?.ProviderVenueId.Should().Be(1);
            model?.Postcode.Should().Be("CV1 2WT");
        }
    }
}