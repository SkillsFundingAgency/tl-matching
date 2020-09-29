using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.InMemoryDb
{
    public class DataBuilder
    {

        public static async Task SetTestData(MatchingDbContext dbContext,
            Provider provider,
            ProviderVenue venue,
            Opportunity opportunity,
            BackgroundProcessHistory backgroundProcessHistory,
            bool isSaved = true, bool isSelectedForReferral = true)
        {
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await dbContext.AddAsync(provider);
            await dbContext.AddAsync(venue);
            await dbContext.AddAsync(opportunity);
            await dbContext.AddAsync(backgroundProcessHistory);
            await dbContext.SaveChangesAsync();

            dbContext.DetachAllEntities();

            var items = dbContext.OpportunityItem
                .AsNoTracking()
                .Where(oi => oi.OpportunityId == opportunity.Id)
                .ToList();

            foreach (var opportunityItem in items)
            {
                opportunityItem.IsSaved = isSaved;
                opportunityItem.IsCompleted = false;
                opportunityItem.OpportunityType = "Referral";
                opportunityItem.IsSelectedForReferral = isSelectedForReferral;

                dbContext.Entry(opportunityItem).Property("IsSaved").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsCompleted").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsSelectedForReferral").IsModified = true;
                dbContext.Entry(opportunityItem).Property("OpportunityType").IsModified = true;
            }

            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();
        }

        public static async Task SetEmailTemplate(
            MatchingDbContext dbContext,
            EmailTemplate emailTemplate)
        {
            await dbContext.AddAsync(emailTemplate);
            
            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();
        }

        public static async Task SetEmailHistory(
            MatchingDbContext dbContext,
            EmailHistory emailHistory)
        {
            emailHistory.Status = null;
            emailHistory.ModifiedBy = null;
            emailHistory.ModifiedOn = null;

            await dbContext.AddAsync(emailHistory);

            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();
        }
    }
}
