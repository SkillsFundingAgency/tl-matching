using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderFeedbackRequestHistory.Builders
{
    public class ValidProviderFeedbackRequestHistoryBuilder
    {
        public Domain.Models.ProviderFeedbackRequestHistory Build() => new Domain.Models.ProviderFeedbackRequestHistory
        {
            Id = 1,
            ProviderCount = 5,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
