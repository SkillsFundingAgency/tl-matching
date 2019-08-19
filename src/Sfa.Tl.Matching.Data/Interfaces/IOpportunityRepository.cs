using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IOpportunityRepository : IRepository<Opportunity>
    {
        Task<IList<OpportunityReferralDto>> GetProviderOpportunities(int opportunityId, IEnumerable<int> itemIds);
        Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId, IEnumerable<int> itemIds);
        Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId);
        Task<OpportunityReportDto> GetPipelineOpportunitiesAsync(int opportunityId);
        int GetEmployerOpportunityCount(int opportunityId);
        Task<IList<EmployerReferralDto>> GetReferralsForEmployerFeedbackAsync(DateTime referralDate);
    }
}
