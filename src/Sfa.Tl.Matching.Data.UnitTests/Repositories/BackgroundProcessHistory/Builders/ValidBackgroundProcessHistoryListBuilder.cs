using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory.Builders
{
    public class ValidBackgroundProcessHistoryListBuilder
    {
        public IList<Domain.Models.BackgroundProcessHistory> Build() => new List<Domain.Models.BackgroundProcessHistory>
        { 
            new Domain.Models.BackgroundProcessHistory
            {
                Id = 1,
                RecordCount = 5,
                Status = BackgroundProcessHistoryStatus.Pending.ToString(),
                StatusMessage = "Status Message",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.BackgroundProcessHistory
            {
                Id = 2,
                RecordCount = 10,
                Status = BackgroundProcessHistoryStatus.Complete.ToString(),
                StatusMessage = "Status Message",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
