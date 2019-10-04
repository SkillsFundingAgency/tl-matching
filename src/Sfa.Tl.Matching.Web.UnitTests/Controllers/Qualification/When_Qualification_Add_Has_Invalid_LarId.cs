using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Add_Has_Invalid_LarId
    {
        private readonly IActionResult _result;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;
        
        public When_Qualification_Add_Has_Invalid_LarId()
        {
            var mapper = Substitute.For<IMapper>();

            var providerVenueService = Substitute.For<IProviderVenueService>();

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.IsValidLarIdAsync("12345").Returns(false);

            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(mapper, providerVenueService, _qualificationService, _providerQualificationService, routePathService);

            var viewModel = new AddQualificationViewModel
            {
                LarId = "12345",
                Postcode = "CV1 2WT"
            };

            _result = qualificationController.AddQualificationAsync(viewModel).GetAwaiter().GetResult();
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
        public void Then_IsValidLarId_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).IsValidLarIdAsync("12345");
        }

        [Fact]
        public void Then_CreateProviderQualificationAsync_Is_Not_Called()
        {
            _providerQualificationService.DidNotReceive().CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }
    }
}