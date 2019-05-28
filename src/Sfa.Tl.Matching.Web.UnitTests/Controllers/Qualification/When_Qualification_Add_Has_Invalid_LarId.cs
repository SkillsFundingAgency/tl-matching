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
        private readonly IQualificationService _qualificationService;
        
        public When_Qualification_Add_Has_Invalid_LarId()
        {
            var providerVenueService = Substitute.For<IProviderVenueService>();

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.IsValidLarIdAsync("12345").Returns(false);
            var qualificationController = new QualificationController(providerVenueService, _qualificationService);

            var viewModel = new AddQualificationViewModel
            {
                LarsId = "12345",
                Postcode = "CV1 2WT"
            };

            _result = qualificationController.AddQualification(viewModel).GetAwaiter().GetResult();
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
        public void Then_CreateVenue_Is_Not_Called()
        {
            _qualificationService.DidNotReceive().CreateQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }
    }
}