using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders
{
    internal class ValidPathListBuilder
    {
        private readonly IList<Domain.Models.Path> _paths;

        public ValidPathListBuilder()
        {
            _paths =
                new List<Domain.Models.Path>
                {
                    new Domain.Models.Path
                    {
                        Id = PathConstants.Id,
                        Name = PathConstants.Name,
                        Keywords = PathConstants.Keywords,
                        Summary = PathConstants.Summary,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    },
                    new Domain.Models.Path
                    {
                        Id = PathConstants.Id + 1,
                        Name = PathConstants.SecondName,
                        Keywords = PathConstants.Keywords,
                        Summary = PathConstants.Summary,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    }
                };
        }

        public IEnumerable<Domain.Models.Path> Build() =>
            _paths;
    }
}
