using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public class ValidEmployerDtoListBuilder
    {
        private readonly IList<EmployerDto> _employerDtos;

        public ValidEmployerDtoListBuilder(int numberOfItems)
        {
            _employerDtos = new List<EmployerDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                _employerDtos.Add(new EmployerDto
                {
                    CrmId  = new Guid("8F7B99CB-0FAD-4FFC-AF6A-D5537293E04B"),
                    CompanyName = "Company Name",
                    AlsoKnownAs  = "Also Known As",
                    Aupa  = "Aware",
                    CompanyType  = "CompanyType",
                    PrimaryContact = "PrimaryContact",
                    Phone = "01777757777",
                    Email  = "primary@contact.co.uk",
                    PostCode  = "AA1 1AA",
                    Owner  = "Owner",
                    CreatedBy = "Test"
                });
            }
        }

        public IEnumerable<EmployerDto> Build() =>
            _employerDtos;
    }
}
