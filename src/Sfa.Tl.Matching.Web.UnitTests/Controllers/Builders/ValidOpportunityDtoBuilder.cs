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
            ModifiedBy = "ModifiedBy"
        };
    }
}