using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders
{
    public class ValidPathListBuilder
    {
        public IList<Domain.Models.Path> Build() => new List<Domain.Models.Path>
        {
            new Domain.Models.Path
            {
                Id = 1,
                Name = "Path 1",
                Keywords = "Keyword",
                Summary = "Path summary",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Path
            {
                Id = 2,
                Name = "Path 2",
                Keywords = "Keyword",
                Summary = "Path summary",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
