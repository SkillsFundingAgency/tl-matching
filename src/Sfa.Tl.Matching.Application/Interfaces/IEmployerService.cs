using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<int> ImportEmployer(EmployerFileImportDto fileImportDto);
        void GetEmployerByName();
        void CreateEmployer();
        void UpdateEmployer();
    }
}