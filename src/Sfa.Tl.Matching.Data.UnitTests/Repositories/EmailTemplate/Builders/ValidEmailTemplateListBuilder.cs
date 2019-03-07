using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders
{
    public class ValidEmailTemplateListBuilder
    {
        public IList<Domain.Models.EmailTemplate> Build() => new List<Domain.Models.EmailTemplate>
        { 
            new Domain.Models.EmailTemplate
            {
                Id = 1,
                TemplateId = "TestTemplateId",
                TemplateName = "TestTemplate",
                Recipients = "test@test.com",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.EmailTemplate
            {
                Id = 2,
                TemplateId = "TestTemplateId2",
                TemplateName = "TestTemplate2",
                Recipients = "test2@test.com",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
