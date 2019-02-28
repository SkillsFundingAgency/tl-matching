using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerDto> GetEmployer(int id);
        IEnumerable<EmployerSearchResultDto> Search(string employerName);
    }
}