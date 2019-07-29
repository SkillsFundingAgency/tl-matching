using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class ValidProvisionGapBuilder
    {
        public ProvisionGap Build() => new ProvisionGap
        {
            Id = 1,
            OpportunityItemId = 1,
            NoSuitableStudent = null,
            HadBadExperience = null,
            ProvidersTooFarAway = null,
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
