using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerDataParser : IDataParser<EmployerDto>
    {
        public EmployerDto Parse(string[] cells)
        {
            return new EmployerDto
            {
                CrmId = cells[EmployerColumnIndex.CrmId].ToGuid(),
                CompanyName = cells[EmployerColumnIndex.CompanyName],
                AlsoKnownAs = cells[EmployerColumnIndex.CompanyAka],
                Aupa = cells[EmployerColumnIndex.Aupa],
                CompanyType = cells[EmployerColumnIndex.CompanyType],
                PrimaryContact = cells[EmployerColumnIndex.PrimaryContact],
                Phone = cells[EmployerColumnIndex.Phone],
                Email = cells[EmployerColumnIndex.Email],
                PostCode = cells[EmployerColumnIndex.PostCode],
                Owner = cells[EmployerColumnIndex.Owner]
            };
        }
    }
}
