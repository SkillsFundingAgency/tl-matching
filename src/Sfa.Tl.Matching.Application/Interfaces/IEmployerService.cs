using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task<bool> ValidateCompanyNameAndId(int employerId, string companyName);
        IEnumerable<EmployerSearchResultDto> Search(string companyName);
        Task<EmployerDetailsViewModel> GetOpportunityEmployerDetailAsync(int opportunityId, int opportunityItemId);
        Task<FindEmployerViewModel> GetOpportunityEmployerAsync(int opportunityId, int opportunityItemId);
        Task<int> GetInProgressEmployerOpportunityCountAsync(string username);
        Task<SavedEmployerOpportunityViewModel> GetSavedEmployerOpportunitiesAsync(string username);

        Task<RemoveEmployerDto> GetConfirmDeleteEmployerOpportunity(int opportunityId, string username);

        Task<string> GetEmployerOpportunityOwnerAsync(int employerId);
    }
}