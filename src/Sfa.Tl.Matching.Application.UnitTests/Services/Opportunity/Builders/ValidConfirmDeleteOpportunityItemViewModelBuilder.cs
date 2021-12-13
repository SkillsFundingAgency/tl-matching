using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class ValidConfirmDeleteOpportunityItemViewModelBuilder
    {
        public ConfirmDeleteOpportunityItemViewModel Build(int basketcount = 1, bool hasPlacement = true) => new()
        {
            OpportunityId = 1,
            OpportunityItemId = 2,
            CompanyName = "CompanyName",
            CompanyNameAka = "AlsoKnownAs",
            Postcode = "Postcode",
            JobRole = "JobRole",
            BasketItemCount = basketcount,
            Placements = hasPlacement ? 1 : null,
        };
    }
}