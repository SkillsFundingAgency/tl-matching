using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        public const string SystemId = "TLevelsIndustryPlacement";
        public const string DefaultReplyToAddress = "DummyAddressOverriddenByNotificationsService"; //reply address is currently ignored by DAS Notifications

        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailService(MatchingConfiguration configuration, 
            INotificationsApi notificationsApi,
            IRepository<EmailTemplate> emailTemplateRepository,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
            _notificationsApi = notificationsApi;
            _logger = logger;
        }

        public async Task SendEmail(string templateName, string toAddress, string subject, IDictionary<string, string> personalisationTokens, string replyToAddress)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == templateName);
            if (emailTemplate == null)
            {
                _logger.LogWarning($"Email template {templateName} not found. No emails sent.");

                return;
            }

            var recipients = new List<string>();
            if (!string.IsNullOrWhiteSpace(toAddress))
            {
                recipients.Add(toAddress.Trim());
            }

            var templateId = emailTemplate.TemplateId;

            replyToAddress = !string.IsNullOrWhiteSpace(replyToAddress)
                ? replyToAddress
                : DefaultReplyToAddress;

            foreach (var recipient in recipients)
            {
                _logger.LogInformation($"Sending {templateName} email to {recipient}");

                await SendEmailViaNotificationsApi(recipient, subject, templateId, personalisationTokens, replyToAddress);
            }
        }

        private async Task SendEmailViaNotificationsApi(string recipient, string subject, string templateId,
            IDictionary<string, string> personalisationTokens, string replyToAddress)
        {
            var email = new SFA.DAS.Notifications.Api.Types.Email
            {
                RecipientsAddress = recipient,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                Subject = subject,
                SystemId = SystemId,
                Tokens = (Dictionary<string, string>)personalisationTokens
            };

            try
            {
                await _notificationsApi.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {templateId} to {recipient}");
            }
        }
    }
}
