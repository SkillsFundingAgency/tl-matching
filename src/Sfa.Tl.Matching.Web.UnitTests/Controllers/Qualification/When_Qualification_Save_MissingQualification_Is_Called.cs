using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Save_MissingQualification_Is_Called
    {
        //private readonly IQualificationService _qualificationService;
        private readonly IActionResult _result;

        public When_Qualification_Save_MissingQualification_Is_Called()
        {
            var qualificationService = Substitute.For<IQualificationService>();

            var providerVenueService = Substitute.For<IProviderVenueService>();

            var qualificationController = new QualificationController(providerVenueService, qualificationService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            _result = controllerWithClaims.MissingQualification(1);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        //[Fact]
        //public void Then_Result_Is_RedirectToRoute()
        //{
        //    var result = _result as RedirectToRouteResult;
        //    result.Should().NotBeNull();
        //    result?.RouteName.Should().Be("GetProviderVenueDetail");
        //}
        
        //[Fact]
        //public void Then_CreateQualification_Is_Called_Exactly_Once()
        //{
        //    _qualificationService.Received(1).CreateQualificationAsync(Arg.Any<AddQualificationViewModel>());
        //}
    }
}