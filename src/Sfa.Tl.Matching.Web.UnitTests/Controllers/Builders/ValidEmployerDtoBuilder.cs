using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidEmployerDtoBuilder
    {
        public EmployerDto Build() => new EmployerDto
        {
            CrmId = new Guid("D7A48843-44CA-46A4-A391-70D7B01C68BC"),
            CompanyName = "CompanyName",
            Aupa = "EmployerAupa",
            Owner = "EmployerOwner",
            PrimaryContact = "EmployerContact",
            Email = "EmployerContactEmail",
            Phone = "EmployerContactPhone"
        };
    }
}