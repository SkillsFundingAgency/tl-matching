using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders
{
    public class ValidQualificationListBuilder
    {
        public IList<Domain.Models.Qualification> Build() => new List<Domain.Models.Qualification>
        {
            new()
            {
                Id = 1,
                LarId = "1000",
                Title = "Title",
                ShortTitle = "ShortTitle",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                LarId = "1001",
                Title = "Title1",
                ShortTitle = "ShortTitle2",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
