using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Create_Post_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityDto _dto = new OpportunityDto();

        public When_Opportunity_Controller_Create_Post_Is_Called()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.CreateOpportunity(Arg.Any<OpportunityDto>()).Returns(opportunityId);

            var opportunityController = new OpportunityController(_opportunityService);
            opportunityController.AddUsernameToContext("username");

            opportunityController.Create(1, "cv12wt", 10).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateOpportunity(Arg.Is<OpportunityDto>(dto => dto.RouteId == 1 && dto.Postcode == "cv12wt" && dto.Distance == 10));
        }
    }
}