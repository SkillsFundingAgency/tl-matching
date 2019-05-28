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
    public class When_Qualification_MissingQualification_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_Qualification_MissingQualification_Is_Loaded()
        {
            var qualificationService = Substitute.For<IQualificationService>();

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.GetVenuePostcodeAsync(1)
                .Returns("CV1 2WT");

            var qualificationController = new QualificationController(_providerVenueService, qualificationService);

            _result = qualificationController.MissingQualification(1);
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
            var viewModel = _result.GetViewModel<MissingQualificationViewModel>();
            viewModel.ProviderVenueId.Should().Be(1);
        }
    }
}