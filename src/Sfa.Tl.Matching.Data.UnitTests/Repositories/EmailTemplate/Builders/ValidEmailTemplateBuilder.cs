using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders
{
    public class ValidEmailTemplateBuilder
    {
        public Domain.Models.EmailTemplate Build() => new Domain.Models.EmailTemplate
        {
            Id = 1,
            TemplateId = "TestTemplateId",
            TemplateName = "TestTemplate",
            Recipients = "test@test.com",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
