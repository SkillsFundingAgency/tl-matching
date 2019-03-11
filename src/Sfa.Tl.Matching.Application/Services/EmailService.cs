using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        //TODO: Rename or move these constants
        public const string SystemId = "TLevelsIndustryPlacement";
        //private const string REPLY_TO_ADDRESS = "digital.apprenticeship.service @notifications.service.gov.uk";

        private readonly ILogger<EmailService> _logger;
        private readonly INotificationsApi _notificationsApi;
        private INotificationClient _govNotifyApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public INotificationClient GovNotifyApi
        {
            get => _govNotifyApi;
            set =>  _govNotifyApi = value;
        }

        public EmailService(INotificationsApi notificationsApi,
            IRepository<EmailTemplate> emailTemplateRepository,
            ILogger<EmailService> logger)
        {
            _logger = logger;

            _emailTemplateRepository = emailTemplateRepository;
            _notificationsApi = notificationsApi;
        }

        public async Task SendEmail(string templateName, string toAddress, string subject, dynamic tokens, string replyToAddress)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == templateName);
            if (emailTemplate == null)
            {
                return;
            }

            var recipients = new List<string>();
            if (!string.IsNullOrWhiteSpace(toAddress))
            {
                recipients.Add(toAddress.Trim());
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.Recipients))
            {
                recipients.AddRange(emailTemplate.Recipients.Split(';').Select(x => x.Trim()));
            }

            var templateId = emailTemplate.TemplateId;

            var personalisationTokens = new Dictionary<string, string>();

            if (tokens != null)
            {
                foreach (var property in tokens.GetType().GetProperties())
                {
                    personalisationTokens[property.Name] = property.GetValue(tokens);
                }
            }

            foreach (var recipient in recipients)
            {
                _logger.LogInformation($"Sending {templateName} email to {recipient}");

                if (_govNotifyApi == null)
                {
                    await SendEmailViaNotificationsApi(recipient, subject, templateId, personalisationTokens, replyToAddress);
                }
                else
                {
                    SendEmailViaGovNotifyApi(recipient, subject, templateId, personalisationTokens, replyToAddress);
                }
            }
        }

        private async Task SendEmailViaNotificationsApi(string recipient, string subject, string templateId,
            Dictionary<string, string> personalisationTokens, string replyToAddress)
        {
            var email = new SFA.DAS.Notifications.Api.Types.Email
            {
                RecipientsAddress = recipient,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                Subject = subject,
                SystemId = SystemId,
                Tokens = personalisationTokens
            };

            try
            {
                await _notificationsApi.SendEmail(email);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error sending email template {templateId} to {toAddress}");
                _logger.LogError(ex, $"Error sending email to {recipient}");
            }
        }

        private void SendEmailViaGovNotifyApi(string recipient, string subject, string templateId,
            Dictionary<string, string> personalisationTokens, string replyToAddress)
        {
            try
            {
                var tokens = new Dictionary<string, dynamic>();
                foreach (var token in personalisationTokens)
                {
                    tokens[token.Key] = token.Value;
                }

                //TODO: Set up replytoid - see on https://docs.notifications.service.gov.uk/net.html#send-an-email-arguments-personalisation-optional
                var response = _govNotifyApi.SendEmail(recipient, templateId, tokens);
                _logger.LogInformation($"Successfully sent email '{response.content.subject}' email to {recipient} from {response.content.fromEmail}");
                _logger.LogInformation($"     Email body: '{response.content.body}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {recipient}");
            }
        }
    }
}
