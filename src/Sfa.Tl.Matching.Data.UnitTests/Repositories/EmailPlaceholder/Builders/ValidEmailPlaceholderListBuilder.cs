using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder.Builders
{
    public class ValidEmailPlaceholderListBuilder
    {
        public IList<Domain.Models.EmailPlaceholder> Build() => new List<Domain.Models.EmailPlaceholder>
        {
            new Domain.Models.EmailPlaceholder
            {
                Id = 1,
                EmailHistoryId = 1,
                Key = "name_Placeholder",
                Value = "Name",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.EmailPlaceholder
            {
                Id = 2,
                EmailHistoryId = 1,
                Key = "address_Placeholder",
                Value = "Address",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
