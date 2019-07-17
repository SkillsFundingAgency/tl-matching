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
        private readonly IOpportunityService _opportunityService;

        private readonly IActionResult _result;

        public When_Employer_Consent_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var referralService = Substitute.For<IReferralService>();
            var mapper = new Mapper(config);

            var employerController = new EmployerController(null, _opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName("username")
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.EmployerConsent(new EmployerConsentViewModel
                {
                    OpportunityId = 1
                }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ConfirmOpportunities_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).ConfirmOpportunities(1);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().BeOfType<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("EmailSentReferrals_Get");
            redirect?.RouteValues["id"].Should().Be(1);
        }
    }
}