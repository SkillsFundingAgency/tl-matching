using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Add_Has_Invalid_OfQual_LarId
    {
        private readonly IActionResult _result;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;

        public When_Qualification_Add_Has_Invalid_OfQual_LarId()
        {
            var providerVenueService = Substitute.For<IProviderVenueService>();

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.IsValidLarIdAsync("12345678").Returns(true);
            _qualificationService.IsValidOfqualLarIdAsync("12345678").Returns(false);

            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(providerVenueService, _qualificationService, _providerQualificationService, routePathService);

            var viewModel = new AddQualificationViewModel
            {
                LarId = "12345678",
                Postcode = "CV1 2WT"
            };

            _result = qualificationController.CreateQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }


        [Fact]
        public void Then_Model_Contains_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["LarId"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real learning aim reference (LAR)");
        }

        [Fact]
        public void Then_IsValidLarId_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).IsValidLarIdAsync("12345678");
        }

        [Fact]
        public void Then_IsValidOfqualLarId_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).IsValidOfqualLarIdAsync("12345678");
        }

        [Fact]
        public void Then_CreateProviderQualificationAsync_Is_Not_Called()
        {
            _providerQualificationService.DidNotReceive().CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }
    }
}