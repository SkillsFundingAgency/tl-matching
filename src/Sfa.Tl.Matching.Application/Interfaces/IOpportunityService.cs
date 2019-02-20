using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityService
    {
        Task<int> CreateOpportunity(OpportunityDto opportunityDto);
        Task UpdateOpportunity(OpportunityDto opportunityDto);
        Task<OpportunityDto> GetOpportunity(int id);
    }
}