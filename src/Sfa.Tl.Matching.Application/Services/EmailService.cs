using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        //TODO: Rename or move these constants
        //TODO: Pass some of these in configuration or as parameters to the send method
        public const string SystemId = "TLevelsIndustryPlacement";
        //private const string REPLY_TO_ADDRESS = "digital.apprenticeship.service @notifications.service.gov.uk";

        private readonly ILogger<EmailService> _logger;
        //private readonly MatchingConfiguration _configuration;
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailService(INotificationsApi notificationsApi,
            //MatchingConfiguration configuration,
            IRepository<EmailTemplate> emailTemplateRepository,
            ILogger<EmailService> logger)
        {
            //TODO: Is _configuration required?
            //_configuration = configuration;
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

            foreach (var property in tokens.GetType().GetProperties())
            {
                personalisationTokens[property.Name] = property.GetValue(tokens);
            }

            foreach (var recipient in recipients)
            {
                _logger.LogInformation($"Sending {templateName} email to {recipient}");

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
        }
    }
}
