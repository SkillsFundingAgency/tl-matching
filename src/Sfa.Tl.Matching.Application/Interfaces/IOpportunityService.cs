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
        Task<OpportunityDto> GetOpportunity(int opportunityId);
        Task<OpportunityItemDto> GetOpportunityItem(int opportunityItemId);
        Task<PlacementInformationSaveDto> GetPlacementInformationSaveAsync(int opportunityItemId);
        List<ReferralDto> GetReferrals(int opportunityItemId);
        OpportunityDto GetLatestCompletedOpportunity(int employerId);
        Task<bool> IsReferralOpportunityItemAsync(int id);
        Task<CheckAnswersDto> GetCheckAnswers(int id);
        Task UpdateOpportunity<T>(T dto) where T : BaseOpportunityUpdateDto;
        Task UpdateOpportunityItemAsync<T>(T dto) where T : BaseOpportunityUpdateDto;
        Task UpdateReferrals(OpportunityDto dto);
        Task<bool> IsNewReferralAsync(int opportunityItemId);
        Task<bool> IsNewProvisionGapAsync(int opportunityItemId);
        Task<int> GetOpportunityItemCountAsync(int opportunityId);
        Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId);
    }
}