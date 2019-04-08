using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerDataParser : IDataParser<EmployerDto>
    {
        public IEnumerable<EmployerDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is EmployerFileImportDto data)) return null;
            
            var employerDto = new EmployerDto
            {
                CrmId = data.CrmId.ToGuid(),
                CompanyName =  data.CompanyName.ToTitleCase(),
                AlsoKnownAs = data.AlsoKnownAs.ToTitleCase(),
                CompanyNameSearch = data.CompanyName.ToLetterOrDigit() + data.AlsoKnownAs.ToLetterOrDigit(),
                Aupa = data.Aupa,
                CompanyType = data.CompanyType,
                PrimaryContact = data.PrimaryContact,
                Email = data.Email,
                Phone = data.Phone,
                Postcode = data.Postcode,
                Owner = data.Owner,
                CreatedBy = data.CreatedBy
            };

            return new List<EmployerDto> { employerDto };
        }
    }
}
