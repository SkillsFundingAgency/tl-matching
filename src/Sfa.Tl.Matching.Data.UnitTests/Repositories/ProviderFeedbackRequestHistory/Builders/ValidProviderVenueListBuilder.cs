using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory.Builders
{
    public class ValidProviderFeedbackRequestHistoryListBuilder
    {
        public IList<Domain.Models.ProviderFeedbackRequestHistory> Build() => new List<Domain.Models.ProviderFeedbackRequestHistory>
        { 
            new Domain.Models.ProviderFeedbackRequestHistory
            {
                Id = 1,
                ProviderCount = 5,
                Status = 0,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.ProviderFeedbackRequestHistory
            {
                Id = 2,
                ProviderCount = 10,
                Status = 1,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
