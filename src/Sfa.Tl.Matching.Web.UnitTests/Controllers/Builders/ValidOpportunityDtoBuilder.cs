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
                EmployerCrmId = new Guid("D7A48843-44CA-46A4-A391-70D7B01C68BC"),
                EmployerName = "EmployerName",
                EmployerAupa = "EmployerAupa",
                EmployerOwner = "EmployerOwner",
                Contact = "Contact",
                ContactEmail = "ContactEmail",
                ContactPhone = "ContactPhone",
                ModifiedBy = "ModifiedBy"
            };
        }

        public OpportunityDto Build() =>
            _dto;
    }
}