using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Create_ProvisionGap_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_Opportunity_Controller_Create_ProvisionGap_Is_Called()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.CreateOpportunity(Arg.Any<OpportunityDto>()).Returns(opportunityId);

            var opportunityController = new OpportunityController(_opportunityService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            controllerWithClaims.CreateProvisionGap(0, 1, "cv12wt", 10).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateOpportunity(Arg.Any<OpportunityDto>());
        }
    }
}