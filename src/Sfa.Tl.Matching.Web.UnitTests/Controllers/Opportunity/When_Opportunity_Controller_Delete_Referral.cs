using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Delete_Referral
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;
        private const int OpportunityItemId = 2;
        private const int ReferralIdToDelete = 3;

        public When_Opportunity_Controller_Delete_Referral()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(CheckAnswersDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.DeleteReferralAsync(ReferralIdToDelete, OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Delete_Referral_Result_Is_Correct()
        {
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            _result.Should().NotBeNull();

            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetCheckAnswers");
            result?.RouteValues["opportunityItemId"].Should().Be(OpportunityItemId);
        }

        [Fact]
        public void Then_DeleteReferralAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _opportunityService.Received(1).DeleteReferralAsync(ReferralIdToDelete);
        }
    }
}
