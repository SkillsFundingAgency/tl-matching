using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;

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
            await dbContext.AddAsync(provider);
            await dbContext.AddAsync(venue);
            await dbContext.AddAsync(opportunity);
            await dbContext.AddAsync(backgroundProcessHistory);
            await dbContext.SaveChangesAsync();

            var items = dbContext.OpportunityItem.Where(oi => oi.OpportunityId == opportunity.Id).AsNoTracking()
                .ToList();

            foreach (var opportunityItem in items)
            {
                opportunityItem.IsSaved = isSaved;
                opportunityItem.IsCompleted = false;
                opportunityItem.IsSelectedForReferral = isSelectedForReferral;

                dbContext.Entry(opportunityItem).Property("IsSaved").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsCompleted").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsSelectedForReferral").IsModified = true;
            }

            await dbContext.SaveChangesAsync();
        }

        public static async Task SetEmailTemplate(
            MatchingDbContext dbContext,
            EmailTemplate emailTemplate)
        {
            await dbContext.AddAsync(emailTemplate);
            
            await dbContext.SaveChangesAsync();

        }
    }
}
