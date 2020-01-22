using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Consent_Is_Submitted_Successfully : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Employer_Consent_Is_Submitted_Successfully(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims(_fixture.ModifiedBy);

            _result = controllerWithClaims.SaveEmployerConsentAsync(new EmployerConsentViewModel
            {
                OpportunityId = 1
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ConfirmOpportunities_Is_Called_Exactly_Once()
        {
            _fixture.ReferralService.Received(2).ConfirmOpportunitiesAsync(1, _fixture.ModifiedBy);
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