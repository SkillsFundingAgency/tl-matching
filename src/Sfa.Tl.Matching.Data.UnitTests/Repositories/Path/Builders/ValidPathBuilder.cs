using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders
{
    internal class ValidPathBuilder
    {
        private readonly Domain.Models.Path _path;

        public ValidPathBuilder()
        {
            _path = new Domain.Models.Path
            {
                Id = PathConstants.Id,
                Name = PathConstants.Name,
                Keywords = PathConstants.Keywords,
                Summary = PathConstants.Summary,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.Path Build() =>
            _path;
    }
}
