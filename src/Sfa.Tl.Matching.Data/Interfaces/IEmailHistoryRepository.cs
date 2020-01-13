using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IEmailHistoryRepository : IRepository<EmailHistory>
    {
        Task<List<PotentialOpportunityItems>> GetPotentialOpportunityItemsPrimary();
        Task<List<PotentialOpportunityItems>> GetPotentialOpportunityItemsSecondary();
    }
}