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
    public class When_ProviderVenue_Add_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_ProviderVenue_Add_Is_Loaded()
        {
            var providerVenueService = Substitute.For<IProviderVenueService>();

            var providerVenueController = new ProviderVenueController(providerVenueService);

            _result = providerVenueController.AddProviderVenue(1);
        }
        
        [Fact]
        public void Then_ProviderId_Is_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<AddProviderVenueViewModel>();
            viewModel.ProviderId.Should().Be(1);
        }
    }
}