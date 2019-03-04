using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders
{
    public class ValidQualificationBuilder
    {
        private readonly Domain.Models.Qualification _qualification;

        public ValidQualificationBuilder()
        {
            _qualification = new Domain.Models.Qualification
            {
                Id = 1,
                LarsId = "1000",
                Title = "Title",
                ShortTitle = "ShortTitle",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.Qualification Build() =>
            _qualification;
    }
}
