using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityService
    {
        Task<int> CreateOpportunity(OpportunityDto opportunityDto);
        Task<OpportunityDto> GetOpportunity(int id);
        Task<int> CreateProvisionGap(CheckAnswersProvisionGapViewModel dto);
        Task<int> CreateReferral(CheckAnswersReferralViewModel dto);
        Task<PlacementInformationSaveDto> GetPlacementInformationSave(int id);
        List<ReferralDto> GetReferrals(int opportunityId);
        Task<bool> IsReferralOpportunity(int id);
        Task<CheckAnswersDto> GetCheckAnswers(int id);
        Task Save<T>(T dto) where T : BaseOpportunityUpdateDto;
    }
}