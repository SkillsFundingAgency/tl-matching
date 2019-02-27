using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        private readonly OpportunityDto _dto;

        public ValidOpportunityDtoBuilder()
        {
            _dto = new OpportunityDto
            {
                EmployerName = "EmployerName",
                EmployerContact = "Contact",
                EmployerContactEmail = "ContactEmail",
                EmployerContactPhone = "ContactPhone",
                ModifiedBy = "ModifiedBy"
            };
        }

        public OpportunityDto Build() =>
            _dto;
    }
}