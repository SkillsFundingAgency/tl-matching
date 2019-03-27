using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidEmailTemplateBuilder
    {
        public EmailTemplate Build() => new EmailTemplate
        {
            Id = 1,
            TemplateId = new Guid("F271263E-E3F5-4F6C-9061-2E25B9C0E106").ToString(),
            TemplateName = "TestTemplate"
        };
    }
}
