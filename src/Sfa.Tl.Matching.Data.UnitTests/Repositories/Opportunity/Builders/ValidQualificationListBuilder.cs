using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidQualificationListBuilder
    {
        public IList<Domain.Models.Qualification> Build() => new List<Domain.Models.Qualification>
        { 
            new Domain.Models.Qualification
            {
                Id = 1,
                LarsId = "1001",
                Title = "Title",
                ShortTitle = "Short Title",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Qualification
            {
                Id = 2,
                LarsId = "1002",
                Title = "Title 2",
                ShortTitle = "Duplicate Short Title",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Qualification
            {
                Id = 3,
                LarsId = "1003",
                Title = "Title 3",
                ShortTitle = "Duplicate Short Title",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }        };
    }
}
