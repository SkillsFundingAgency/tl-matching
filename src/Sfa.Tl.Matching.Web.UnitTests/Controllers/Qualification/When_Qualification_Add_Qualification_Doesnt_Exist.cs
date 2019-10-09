using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Add_Qualification_Doesnt_Exist
    {
        private readonly IActionResult _result;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;

        public When_Qualification_Add_Qualification_Doesnt_Exist()
        {
            var mapper = Substitute.For<IMapper>();

            var providerVenueService = Substitute.For<IProviderVenueService>();

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.IsValidLarIdAsync("12345678").Returns(true);
            _qualificationService.GetQualificationAsync("12345678").ReturnsNull();
            _qualificationService.IsValidOfqualLarIdAsync("12345678").Returns(true);

            _providerQualificationService = Substitute.For<IProviderQualificationService>();

            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(mapper, providerVenueService, _qualificationService, _providerQualificationService, routePathService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new AddQualificationViewModel
            {
                ProviderVenueId = 1,
                LarId = "12345678",
                Postcode = "CV1 2WT"
            };

            _result = controllerWithClaims.CreateQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Redirect_Result_Is_Returned() =>
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
            result?.RouteName.Should().Be("MissingQualification");
        }

        [Fact]
        public void Then_RouteValues_Has_Expected_Parameters()
        {
            var result = _result as RedirectToRouteResult;
            result?.RouteValues["providerVenueId"].Should().Be(1);
            result?.RouteValues["larId"].Should().Be("12345678");
        }

        [Fact]
        public void Then_IsValidLarId_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).IsValidLarIdAsync("12345678");
        }

        [Fact]
        public void Then_CreateProviderQualification_Is_Not_Called()
        {
            _providerQualificationService.DidNotReceive().CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }
    }
}