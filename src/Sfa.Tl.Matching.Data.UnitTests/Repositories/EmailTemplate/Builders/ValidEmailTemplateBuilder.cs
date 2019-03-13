using System;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders
{
    public class ValidEmailTemplateBuilder
    {
        public Domain.Models.EmailTemplate Build() => new Domain.Models.EmailTemplate
        {
            Id = 1,
            TemplateId = new Guid("1777EF96-70F5-4537-A6B1-41E8A0D8E0EC").ToString(),
            TemplateName = "TestTemplate",
            Recipients = "test@test.com",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
