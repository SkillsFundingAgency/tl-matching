using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory.Builders
{
    public class ValidBackgroundProcessHistoryBuilder
    {
        public Domain.Models.BackgroundProcessHistory Build() => new()
        {
            Id = 1,
            RecordCount = 5,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
