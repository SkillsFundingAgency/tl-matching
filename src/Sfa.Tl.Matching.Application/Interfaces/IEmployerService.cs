using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<bool> ValidateEmployerNameAndId(int employerId, string companyName);
        IEnumerable<EmployerSearchResultDto> Search(string employerName);
        Task<EmployerDetailsViewModel> GetOpportunityEmployerDetailAsync(int opportunityId, int opportunityItemId);
        Task<FindEmployerViewModel> GetOpportunityEmployerAsync(int opportunityId, int opportunityItemId);
    }
}