using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class EmailHistoryRepository : GenericRepository<EmailHistory>, IEmailHistoryRepository
    {
        public EmailHistoryRepository(ILogger<EmailHistoryRepository> logger, MatchingDbContext dbContext) : base(
            logger, dbContext)
        {
        }

        public async Task<List<PotentialOpportunityItems>> GetPotentialOpportunityItemsPrimary()
        {
            var potentialOpportunityItems = await (from eh in _dbContext.EmailHistory
                join o in _dbContext.Opportunity
                    on eh.OpportunityId equals o.Id
                join oi in _dbContext.OpportunityItem
                    on o.Id equals oi.OpportunityId
                join p in _dbContext.Provider
                    on eh.SentTo equals p.PrimaryContactEmail
                where eh.OpportunityId.HasValue
                orderby eh.Id
                select new PotentialOpportunityItems
                {
                    EmailHistoryId = eh.Id,
                    OpportunityId = eh.OpportunityId.Value,
                    OpportunityItemId = oi.Id,
                    SentTo = eh.SentTo,
                    ContactEmail = p.PrimaryContactEmail,
                    EmailHistoryCreated = eh.CreatedOn,
                    EmailHistoryModified = eh.ModifiedOn,
                    OpportunityItemCreated = oi.CreatedOn,
                    OpportunityItemModified = oi.ModifiedOn,
                    ProviderCreated = p.CreatedOn,
                    ProviderModified = p.ModifiedOn
                }).ToListAsync();

            return potentialOpportunityItems;
        }

        public async Task<List<PotentialOpportunityItems>> GetPotentialOpportunityItemsSecondary()
        {
            var potentialOpportunityItems = await (from eh in _dbContext.EmailHistory
                join o in _dbContext.Opportunity
                    on eh.OpportunityId equals o.Id
                join oi in _dbContext.OpportunityItem
                    on o.Id equals oi.OpportunityId
                join p in _dbContext.Provider
                    on eh.SentTo equals p.SecondaryContactEmail
                where eh.OpportunityId.HasValue
                orderby eh.Id
                select new PotentialOpportunityItems
                {
                    EmailHistoryId = eh.Id,
                    OpportunityId = eh.OpportunityId.Value,
                    OpportunityItemId = oi.Id,
                    SentTo = eh.SentTo,
                    ContactEmail = p.SecondaryContactEmail,
                    EmailHistoryCreated = eh.CreatedOn,
                    EmailHistoryModified = eh.ModifiedOn,
                    OpportunityItemCreated = oi.CreatedOn,
                    OpportunityItemModified = oi.ModifiedOn,
                    ProviderCreated = p.CreatedOn,
                    ProviderModified = p.ModifiedOn
                }).ToListAsync();

            return potentialOpportunityItems;
        }
    }
}