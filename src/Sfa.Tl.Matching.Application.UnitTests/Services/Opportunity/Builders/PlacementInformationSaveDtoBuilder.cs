using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class PlacementInformationSaveDtoBuilder
    {
        public PlacementInformationSaveDto Build() => new PlacementInformationSaveDto
        {
            OpportunityId = 1,
            OpportunityItemId = 2,
            JobRole = "Test Job Role",
            PlacementsKnown = true,
            Placements = 5,
            NoSuitableStudent = true,
            HadBadExperience = true,
            ProvidersTooFarAway = true
        };
    }
}
