using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Controller_ConfirmProviderVenueChange_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;

        private const long UkPrn = 10000546;
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenue_Controller_ConfirmProviderVenueChange_Is_Loaded()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService
                .GetProviderByUkPrnAsync(Arg.Any<long>())
                .Returns(new ValidProviderDtoBuilder().Build());

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenueWithQualificationsAsync(Arg.Any<long>(), Arg.Any<string>())
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = 1,
                    UkPrn = 10000546,
                    Postcode = "CV1 2WT",
                    ProviderId = 1,
                    ProviderName = "Test Provider",
                    Source = "Admin",
                    IsEnabledForSearch = true,
                    VenueName = "VenueName"
                });

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueDtoMapper).Assembly));
            var mapper = new Mapper(config);
            var providerController = new ProviderVenueController(mapper, _providerService, _providerVenueService);

            _result = providerController.ConfirmProviderVenueChange(10000546, "CV1 2WT").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueService_GetVenueWithQualifications_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetVenueWithQualificationsAsync(UkPrn, Postcode);
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
        public void Then_Model_UkPrn_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.UkPrn.Should().Be(10000546);
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
            model.ProviderName.Should().Be("Test Provider");
        }

        [Fact]
        public void Then_Model_IsEnabledForSearch_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as HideProviderVenueViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.IsEnabledForSearch.Should().Be(true);
        }
    }
}