using System;
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
                TemplateId = new Guid("1777EF96-70F5-4537-A6B1-41E8A0D8E0EC").ToString(),
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
                TemplateId = new Guid("02D150D6-BD64-4450-A82E-C67DFD5FAC09").ToString(),
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
