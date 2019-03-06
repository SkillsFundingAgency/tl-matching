using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityService
    {
        Task<int> CreateOpportunity(OpportunityDto opportunityDto);
        Task UpdateOpportunity(OpportunityDto opportunityDto);
        Task<OpportunityDto> GetOpportunity(int id);
        Task<int> CreateProvisionGap(CheckAnswersViewModel dto);
        Task<int> CreateReferal(int opportunityId);
        Task SavePlacementInformation(PlacementInformationViewModel dto);
    }
}