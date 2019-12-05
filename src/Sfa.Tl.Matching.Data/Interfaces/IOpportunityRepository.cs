using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IOpportunityRepository : IRepository<Opportunity>
    {
        Task<IList<OpportunityReferralDto>> GetProviderOpportunitiesAsync(int opportunityId, IEnumerable<int> itemIds);
        Task<EmployerReferralDto> GetEmployerReferralsAsync(int opportunityId, IEnumerable<int> itemIds);
        Task<OpportunityBasketViewModel> GetOpportunityBasketAsync(int opportunityId);
        Task<OpportunityReportDto> GetPipelineOpportunitiesAsync(int opportunityId);
        int GetEmployerOpportunityCount(int opportunityId);
        Task<IList<MatchingServiceOpportunityReport>> GetMatchingServiceOpportunityReportAsync();
        Task<IList<MatchingServiceProviderOpportunityReport>> GetMatchingServiceProviderOpportunityReportAsync();
        Task<IList<MatchingServiceProviderEmployerReport>> GetMatchingServiceProviderEmployerReportAsync();
        Task<EmailBodyDto> GetEmailDeliveryStatusForEmployerAsync(int opportunityId, string sentTo);
        Task<EmailBodyDto> GetEmailDeliveryStatusForProviderAsync(int opportunityId, string sentTo);
    }
}
