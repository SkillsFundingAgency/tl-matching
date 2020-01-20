using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class EmailTemplateData
    {
        internal static EmailTemplate[] Create()
        {
            var emailTemplates = new[]
            {
                new EmailTemplate
                {
                    //Id = 1,
                    TemplateName = "ProviderReferral",
                    TemplateId = "5740b7d4-b421-4497-8649-81cd57dbc0b0",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                },
                new EmailTemplate
                {
                    //Id = 2,
                    TemplateName = "EmployerReferral",
                    TemplateId = "4918d3d5-6694-4f11-975f-e91c255dd583",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                },
                new EmailTemplate
                {
                    //Id = 3,
                    TemplateName = "ProviderQuarterlyUpdate",
                    TemplateId = "714e5adb-8f08-4b25-9be8-cb2f3fc66ed6",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                }
            };

            return emailTemplates;
        }
    }
}