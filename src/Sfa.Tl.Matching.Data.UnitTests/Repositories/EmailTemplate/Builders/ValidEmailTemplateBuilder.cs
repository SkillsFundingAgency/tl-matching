using System;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders
{
    public class ValidEmailTemplateBuilder
    {
        public Domain.Models.EmailTemplate Build() => new Domain.Models.EmailTemplate
        {
            Id = 1,
            TemplateId = "TestTemplateId",
            TemplateUniqueId = new Guid("0792406B-2E04-4FDC-A996-E4C724687668"),
            TemplateName = "TestTemplate",
            Recipients = "test@test.com",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
