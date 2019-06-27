using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerStagingDto> GetEmployer(int id);
        IEnumerable<EmployerSearchResultDto> Search(string employerName);
        Task<EmployerDetailsViewModel> GetOpportunityEmployerDetails(int opportunityId);
    }
}