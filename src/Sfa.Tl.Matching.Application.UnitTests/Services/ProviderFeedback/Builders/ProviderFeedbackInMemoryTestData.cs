using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders
{
    public class ProviderFeedbackInMemoryTestData
    {
        public static async Task SetTestData(MatchingDbContext dbContext,
            Domain.Models.Provider provider,
            Domain.Models.ProviderVenue venue,
            Domain.Models.Opportunity opportunity,
            bool isSaved = true, bool isSelectedForReferral = true)
        {
            await dbContext.AddAsync(provider);
            await dbContext.AddAsync(venue);
            await dbContext.AddAsync(opportunity);
            await dbContext.SaveChangesAsync();

            var items = dbContext.OpportunityItem.Where(oi => oi.OpportunityId == opportunity.Id).AsNoTracking()
                .ToList();

            foreach (var opportunityItem in items)
            {
                opportunityItem.IsSaved = isSaved;
                opportunityItem.IsCompleted = true;
                opportunityItem.IsSelectedForReferral = isSelectedForReferral;
                opportunityItem.OpportunityType = "Referral";

                dbContext.Entry(opportunityItem).Property("IsSaved").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsCompleted").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsSelectedForReferral").IsModified = true;
                dbContext.Entry(opportunityItem).Property("OpportunityType").IsModified = true;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
