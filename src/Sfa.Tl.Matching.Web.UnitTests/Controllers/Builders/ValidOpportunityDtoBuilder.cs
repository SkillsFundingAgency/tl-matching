using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        public OpportunityDto Build() => new OpportunityDto
        {
            Id = 1,
            EmployerContact = "EmployerContact",
            EmployerContactEmail = "EmployerContactEmail",
            EmployerContactPhone = "EmployerContactPhone",
            EmployerCrmId = new Guid("60E00C18-3192-4283-BBBA-6EB4885D8618"),
            ModifiedBy = "ModifiedBy"
        };
    }
}