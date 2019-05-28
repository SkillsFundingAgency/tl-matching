using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Add_Qualification_Already_Exists
    {
        private readonly IActionResult _result;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;

        public When_Qualification_Add_Qualification_Already_Exists()
        {
            var providerVenueService = Substitute.For<IProviderVenueService>();

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.IsValidLarIdAsync("12345678").Returns(true);
            _qualificationService.GetQualificationAsync("12345678").Returns(new QualificationDetailViewModel
            {
                Id = 1
            });

            _providerQualificationService = Substitute.For<IProviderQualificationService>();

            var qualificationController = new QualificationController(providerVenueService, _qualificationService, _providerQualificationService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new AddQualificationViewModel
            {
                ProviderVenueId = 1,
                LarsId = "12345678",
                Postcode = "CV1 2WT"
            };

            _result = controllerWithClaims.AddQualification(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetProviderVenueDetail");
        }

        [Fact]
        public void Then_RouteValues_Has_ProviderVenueId()
        {
            var result = _result as RedirectToRouteResult;
            result?.RouteValues["providerVenueId"].Should().Be(1);
        }

        [Fact]
        public void Then_IsValidLarId_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).IsValidLarIdAsync("12345678");
        }


        [Fact]
        public void Then_CreateQualification_Is_Called_Exactly_Once()
        {
            _providerQualificationService.Received(1).CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }
    }
}