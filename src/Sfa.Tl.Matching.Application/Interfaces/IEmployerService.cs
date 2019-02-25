using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<int> ImportEmployer(EmployerFileImportDto fileImportDto);
        Task<IEnumerable<EmployerSearchResultDto>> Search(string employerName);
        void GetEmployerByName();
        Task<EmployerDto> GetEmployer(string companyName, string alsoKnownAs);
        void CreateEmployer();
        void UpdateEmployer();
    }
}