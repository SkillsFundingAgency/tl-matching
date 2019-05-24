using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory.Builders
{
    public class ValidbackgroundProcessHistoryBuilder
    {
        public Domain.Models.BackgroundProcessHistory Build() => new Domain.Models.BackgroundProcessHistory
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
