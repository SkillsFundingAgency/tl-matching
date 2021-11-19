using System;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    internal class FindEmployerViewModelBuilder
    {
        public FindEmployerViewModel BuildWithEmployer() => new()
        {
            OpportunityItemId = 1,
            OpportunityId = 2,
            SelectedEmployerCrmId = new Guid("33333333-3333-3333-3333-333333333333"),
            CompanyName = "CompanyName",
            PreviousCompanyName = "CompanyName"
        };
    }
}
