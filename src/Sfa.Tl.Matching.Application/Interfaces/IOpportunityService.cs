using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityService
    {
        Task<int> CreateOpportunityAsync(OpportunityDto opportunityDto);
        Task<int> CreateOpportunityItemAsync(OpportunityItemDto opportunityItemDto);

        Task<int> GetSavedOpportunityItemCountAsync(int opportunityId);
        Task<int> GetReferredOpportunityItemCountAsync(int opportunityId);
        Task<OpportunityDto> GetOpportunityAsync(int opportunityId);
        Task<OpportunityItemDto> GetOpportunityItemAsync(int opportunityItemId);
        Task<PlacementInformationSaveDto> GetPlacementInformationAsync(int opportunityItemId);
        Task<IList<ReferralDto>> GetReferralsAsync(int opportunityItemId);
        Task<CheckAnswersViewModel> GetCheckAnswersAsync(int id);
        Task<OpportunityBasketViewModel> GetOpportunityBasketAsync(int opportunityId);
        Task<ConfirmDeleteOpportunityItemViewModel> GetConfirmDeleteOpportunityItemAsync(int opportunityItemId);

        Task<bool> IsReferralOpportunityItemAsync(int id);
        Task<bool> IsNewReferralAsync(int opportunityItemId);
        Task<bool> IsNewProvisionGapAsync(int opportunityItemId);

        Task UpdateOpportunityAsync<T>(T dto) where T : BaseOpportunityDto;
        Task UpdateOpportunityItemAsync<T>(T dto) where T : BaseOpportunityDto;
        Task UpdateProvisionGapAsync(PlacementInformationSaveDto dto);
        Task UpdateReferralsAsync(OpportunityItemDto opportunityItemDto);

        Task DeleteOpportunityItemAsync(int opportunityId, int opportunityItemId);
        Task ClearOpportunityItemsSelectedForReferralAsync(int opportunityId);

        Task ContinueWithOpportunitiesAsync(ContinueOpportunityViewModel viewModel);
        Task<string> GetCompanyNameWithAkaAsync(int? opportunityId);
        Task DeleteEmployerOpportunityItemAsync(int opportunityId);
        Task<FileDownloadDto> GetOpportunitySpreadsheetDataAsync(int opportunityId);
        Task DeleteReferralAsync(int referralId);
    }
}