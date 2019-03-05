using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders
{
    public class ValidQualificationListBuilder
    {
        public IList<Domain.Models.Qualification> Build() => new List<Domain.Models.Qualification>
        { 
            new Domain.Models.Qualification
            {
                Id = 1,
                LarsId = "1000",
                Title = "Title",
                ShortTitle = "ShortTitle",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Qualification
            {
                Id = 2,
                LarsId = "1001",
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
