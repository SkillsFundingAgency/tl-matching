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

        Task<int> GetOpportunityItemCountAsync(int opportunityId);
        Task<OpportunityDto> GetOpportunity(int opportunityId);
        Task<OpportunityItemDto> GetOpportunityItem(int opportunityItemId);
        Task<PlacementInformationSaveDto> GetPlacementInformationAsync(int opportunityItemId);
        List<ReferralDto> GetReferrals(int opportunityItemId);
        Task<CheckAnswersViewModel> GetCheckAnswers(int id);
        Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId);

        Task<bool> IsReferralOpportunityItemAsync(int id);
        Task<bool> IsNewReferralAsync(int opportunityItemId);
        Task<bool> IsNewProvisionGapAsync(int opportunityItemId);

        Task UpdateOpportunity<T>(T dto) where T : BaseOpportunityDto;
        Task UpdateOpportunityItemAsync<T>(T dto) where T : BaseOpportunityDto;
        Task UpdateProvisionGapAsync(PlacementInformationSaveDto dto);
        Task UpdateReferrals(OpportunityItemDto opportunityItemDto);

        Task RemoveOpportunityItemAsync(int opportunityId, int opportunityItemId);
        Task ClearOpportunityItemsSelectedForReferralAsync(int opportunityId);
    }
}