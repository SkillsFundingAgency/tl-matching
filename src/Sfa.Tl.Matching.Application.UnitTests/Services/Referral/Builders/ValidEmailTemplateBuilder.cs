using System;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidEmailTemplateBuilder
    {
        public Domain.Models.EmailTemplate Build() => new Domain.Models.EmailTemplate
        {
            Id = 1,
            TemplateId = new Guid("F271263E-E3F5-4F6C-9061-2E25B9C0E106").ToString(),
            TemplateName = "TestTemplate",
            Recipients = "test@test.com"
        };
    }
}
