using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Consent_Is_Submitted_Successfully
    {
        private readonly IReferralService _referralService;

        private readonly IActionResult _result;

        public When_Employer_Consent_Is_Submitted_Successfully()
        {
            var opportunityService = Substitute.For<IOpportunityService>();

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            _referralService = Substitute.For<IReferralService>();
            var mapper = new Mapper(config);

            var employerController = new EmployerController(null, opportunityService, _referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName("username")
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveEmployerConsentAsync(new EmployerConsentViewModel
            {
                OpportunityId = 1
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ConfirmOpportunities_Is_Called_Exactly_Once()
        {
            _referralService.Received(1).ConfirmOpportunitiesAsync(1, "username");
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().BeOfType<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetReferralEmailSent");
            redirect?.RouteValues["id"].Should().Be(1);
        }
    }
}