using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityService
    {
        Task<int> CreateOpportunity(OpportunityDto opportunityDto);
        Task SaveEmployerName(EmployerNameDto dto);
        Task SaveEmployerDetail(EmployerDetailDto dto);
        Task SaveCheckAnswers(CheckAnswersDto dto);
        Task<OpportunityDto> GetOpportunity(int id);
        Task<int> CreateProvisionGap(CheckAnswersProvisionGapViewModel dto);
        Task<int> CreateReferral(CheckAnswersReferralViewModel dto);
        Task<PlacementInformationSaveDto> GetPlacementInformationSave(int id);
        Task SavePlacementInformation(PlacementInformationSaveDto dto);
        List<ReferralDto> GetReferrals(int opportunityId);
        Task<bool> IsReferralOpportunity(int id);
        Task<CheckAnswersDto> GetCheckAnswers(int id);
    }
}