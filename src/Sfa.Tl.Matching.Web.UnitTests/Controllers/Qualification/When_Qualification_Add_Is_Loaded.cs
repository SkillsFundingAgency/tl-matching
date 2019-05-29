using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Add_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_Qualification_Add_Is_Loaded()
        {
            var mapper = Substitute.For<IMapper>();

            var qualificationService = Substitute.For<IQualificationService>();

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenuePostcodeAsync(1)
                .Returns("CV1 2WT");

            var providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(mapper, _providerVenueService, qualificationService, providerQualificationService, routePathService);

            _result = qualificationController.AddQualification(1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetVenuePostcodeAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).GetVenuePostcodeAsync(1);
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
        public void Then_ViewModel_Fields_Are_Set()
        {
            var viewModel = _result.GetViewModel<AddQualificationViewModel>();
            viewModel.ProviderVenueId.Should().Be(1);
            viewModel.Postcode.Should().Be("CV1 2WT");
            viewModel.LarId.Should().BeNullOrEmpty();
        }
    }
}