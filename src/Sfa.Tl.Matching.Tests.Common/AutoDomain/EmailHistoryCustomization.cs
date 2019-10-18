using System;
using AutoFixture;
using Notify.Models.Responses;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class EmailHistoryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var opportunity = fixture.Create<Opportunity>();
            var emailTemplate = fixture.Create<EmailTemplate>();
            var notificationId = Guid.NewGuid();

            fixture.Customize<EmailHistory>(composer => composer
                .With(em => em.Opportunity, opportunity)
                .With(em => em.EmailTemplateId, emailTemplate.Id)
                .With(em => em.Status, string.Empty)
                .With(em => em.ModifiedBy, () => null)
                .With(em => em.ModifiedOn, () => null)
            );

            fixture.Customize<EmailNotificationResponse>(composer => composer
                .With(response => response.id, notificationId.ToString));
        }
    }
}