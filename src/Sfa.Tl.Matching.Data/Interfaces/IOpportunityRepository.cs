using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IOpportunityRepository : IRepository<Opportunity>
    {
        Task<IList<OpportunityReferralDto>> GetProviderOpportunities(int opportunityId);
        Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId);
        Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId);
    }
}
