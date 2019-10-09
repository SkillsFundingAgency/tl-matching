using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailService(MatchingConfiguration configuration,
            IAsyncNotificationClient notificationClient,
            IRepository<EmailTemplate> emailTemplateRepository,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
            _notificationClient = notificationClient;
            _logger = logger;

        }

        public async Task SendEmailAsync(string templateName, string toAddress, IDictionary<string, string> personalisationTokens)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefaultAsync(t => t.TemplateName == templateName);
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

            foreach (var recipient in recipients)
            {
                _logger.LogInformation($"Sending {templateName} email to {recipient}");

                await SendEmailViaNotificationsApiAsync(recipient, templateId, personalisationTokens);
            }
        }

        private async Task SendEmailViaNotificationsApiAsync(string recipient, string templateId, 
            IDictionary<string, string> personalisationTokens)
        {
            try
            {
                var tokens = personalisationTokens.Select(x => new { key = x.Key, val = (dynamic)x.Value })
                    .ToDictionary(item => item.key, item => item.val);

                await _notificationClient.SendEmailAsync(recipient, templateId, tokens);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {templateId} to {recipient}");
            }
        }
    }
}