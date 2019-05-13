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
    public class When_ProviderVenue_Controller_ConfirmProviderVenueChange_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Controller_ConfirmProviderVenueChange_Is_Loaded()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetHideProviderVenueViewModelAsync(Arg.Any<int>())
                .Returns(new ValidHideProviderVenueViewModelBuilder().Build());
            
            var providerVenueController = new ProviderVenueController(_providerVenueService);

            _result = providerVenueController.ConfirmProviderVenueChange(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueService_GetHideProviderVenueViewModelAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetHideProviderVenueViewModelAsync(1);
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
        public void Then_Model_Is_Of_Type_HideProviderVenueViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<HideProviderVenueViewModel>();
        }
        
        [Fact]
        public void Then_Model_ProviderVenueId_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.ProviderVenueId.Should().Be(1);
        }
        
        [Fact]
        public void Then_Model_Postcode_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.Postcode.Should().Be("CV1 2WT");
        }

        [Fact]
        public void Then_Model_ProviderName_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.ProviderVenueName.Should().Be("Test Provider");
        }

        [Fact]
        public void Then_Model_IsEnabledForSearch_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.IsRemoved.Should().BeTrue();
        }
    }
}