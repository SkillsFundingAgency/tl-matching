using System.Collections.Generic;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerDataParser : IDataParser<EmployerDto>
    {
        public IEnumerable<EmployerDto> Parse(FileImportDto dto)
        {
            if (!(dto is EmployerFileImportDto data)) return null;

            var employerDto = new EmployerDto
            {
                CrmId = data.CrmId.ToGuid(),
                CompanyName = data.CompanyName,
                AlsoKnownAs = data.AlsoKnownAs,
                Aupa = data.Aupa,
                CompanyType = data.CompanyType,
                PrimaryContact = data.PrimaryContact,
                Email = data.Email,
                Phone = data.Phone,
                PostCode = data.PostCode,
                Owner = data.Owner,
                CreatedBy = data.CreatedBy
            };

            return new List<EmployerDto> { employerDto };
        }
    }
}
