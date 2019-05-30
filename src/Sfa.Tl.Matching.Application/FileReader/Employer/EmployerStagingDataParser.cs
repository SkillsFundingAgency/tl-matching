using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerStagingDataParser : IDataParser<EmployerStagingDto>
    {
        public IEnumerable<EmployerStagingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is EmployerStagingFileImportDto data)) return null;
            
            var employerStagingDto = new EmployerStagingDto
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

            return new List<EmployerStagingDto> { employerStagingDto };
        }
    }
}
