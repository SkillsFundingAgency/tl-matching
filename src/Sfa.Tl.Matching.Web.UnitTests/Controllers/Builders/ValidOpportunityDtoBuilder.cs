using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        public OpportunityDto Build() => new OpportunityDto
        {
            Id = 1,
            CompanyName = "CompanyName",
            PrimaryContact = "EmployerContact",
            Email = "EmployerContactEmail",
            Phone = "EmployerContactPhone",
            EmployerCrmId = new Guid("60E00C18-3192-4283-BBBA-6EB4885D8618"),
            ModifiedBy = "ModifiedBy"
        };
    }
}