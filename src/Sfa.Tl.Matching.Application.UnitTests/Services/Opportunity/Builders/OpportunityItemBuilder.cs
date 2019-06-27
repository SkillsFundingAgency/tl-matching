using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityItemBuilder
    {
        public FindEmployerViewModel BuildWithEmployer() => new FindEmployerViewModel
        {
            OpportunityItemId = 1,
            OpportunityId = 2,
            SelectedEmployerId = 3,
            CompanyName = "CompanyName",
            PreviousCompanyName = "CompanyName",
        };
    }
}
