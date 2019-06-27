using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityItemBuilder
    {
        public OpportunityItem BuildWithEmployer() => new OpportunityItem
        {
            Id = 1,
            OpportunityId = 2,
            Opportunity = new Domain.Models.Opportunity
            {
                Id = 2,
                EmployerId = 3,
                Employer = new Domain.Models.Employer
                {
                    Id = 3,
                    CompanyName = "CompanyName",
                    CreatedBy = "CreatedBy",
                    ModifiedBy = "ModifiedBy"
                },
                CreatedBy = "CreatedBy"
            },
            CreatedBy = "CreatedBy"
        };
    }
}
