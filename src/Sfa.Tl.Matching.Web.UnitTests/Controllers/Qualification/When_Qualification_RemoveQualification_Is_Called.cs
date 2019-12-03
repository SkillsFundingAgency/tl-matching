using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_RemoveQualification_Is_Called
    {
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IActionResult _result;

        public When_Qualification_RemoveQualification_Is_Called()
        {
            var qualificationService = Substitute.For<IQualificationService>();
            var providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(providerVenueService, qualificationService,
                _providerQualificationService, routePathService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            _result = controllerWithClaims.RemoveQualificationAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            _result.Should().NotBeNull();
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetProviderVenueDetail");
            result?.RouteValues["providerVenueId"].Should().Be(1);
        }

        [Fact]
        public void Then_RemoveProviderQualification_Is_Called_With_Expected_ParametersExactly_Once()
        {
            _providerQualificationService.Received(1).RemoveProviderQualificationAsync(
                Arg.Is<int>(p => p == 1),
                Arg.Is<int>(q => q == 2));
        }
    }
}