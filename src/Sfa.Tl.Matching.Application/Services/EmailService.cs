using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notify.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.NotificationCallback;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IMapper _mapper;

        public EmailService(MatchingConfiguration configuration,
            IAsyncNotificationClient notificationClient,
            IRepository<EmailTemplate> emailTemplateRepository,
            IRepository<EmailHistory> emailHistoryRepository,
            IMapper mapper,
            ILogger<EmailService> logger)
        {
            _emailHistoryRepository = emailHistoryRepository;
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
            _notificationClient = notificationClient;
            _logger = logger;

        }

        public async Task SendEmailAsync(int? opportunityId, string templateName, string toAddress, IDictionary<string, string> personalisationTokens, string createdBy)
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

            foreach (var recipient in recipients)
            {
                _logger.LogInformation($"Sending {templateName} email to {recipient}");

                await SendEmailViaNotificationsApiAsync(opportunityId, recipient, emailTemplate, personalisationTokens, createdBy);
            }
        }

        public async Task<int> HandleEmailStatusAsync(string payload)
        {
            var callbackData = JsonConvert.DeserializeObject<CallbackPayLoad>(payload, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            var data = await _emailHistoryRepository.GetFirstOrDefaultAsync(history =>
                history.NotificationId == callbackData.id);

            if (data == null) return -1;

            data.Status = callbackData.status;
            data.ModifiedOn = DateTime.UtcNow;
            data.ModifiedBy = "System";

            await _emailHistoryRepository.UpdateWithSpecifedColumnsOnlyAsync(data,
                history => history.Status,
                history => history.ModifiedOn,
                history => history.ModifiedBy);

            return 1;

        }

        public async Task<FailedEmailDto> GetFailedEmailAsync(string notificationId)
        {
            var notification = await _notificationClient.GetNotificationByIdAsync(notificationId);

            var dto = _mapper.Map<FailedEmailDto>(notification);

            return dto;
        }
		
        private async Task SendEmailViaNotificationsApiAsync(int? opportunityId, string recipient, EmailTemplate emailTemplate,
            IDictionary<string, string> personalisationTokens, string createdBy)
        {
            try
            {
                var tokens = personalisationTokens.Select(x => new { key = x.Key, val = (dynamic)x.Value })
                    .ToDictionary(item => item.key, item => item.val);

                var emailresponse = await _notificationClient.SendEmailAsync(recipient, emailTemplate.TemplateId, tokens);

                await SaveEmailHistoryAsync(emailresponse.id, emailTemplate.Id, personalisationTokens, opportunityId,
                    recipient, createdBy);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {emailTemplate.TemplateId} to {recipient}");
            }
        }

        private async Task SaveEmailHistoryAsync(
            string notificationId,
            int emailTemplateId,
            IDictionary<string, string> tokens,
            int? opportunityId,
            string emailAddress,
            string createdBy)
        {
            var placeholders = ConvertTokensToEmailPlaceholderDtos(tokens, createdBy);
            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);

            Guid.TryParse(notificationId, out var emailNotificationId);

            _logger.LogInformation($"Saving {emailPlaceholders.Count} {nameof(EmailPlaceholder)} items.");

            var emailHistory = new EmailHistory
            {
                NotificationId = emailNotificationId,
                OpportunityId = opportunityId,
                EmailTemplateId = emailTemplateId,
                EmailPlaceholder = emailPlaceholders,
                SentTo = emailAddress,
                CreatedBy = createdBy
            };

            await _emailHistoryRepository.CreateAsync(emailHistory);
        }

        private static IEnumerable<EmailPlaceholderDto> ConvertTokensToEmailPlaceholderDtos(IDictionary<string, string> tokens, string createdBy)
        {
            var placeholders = new List<EmailPlaceholderDto>();

            foreach (var (key, value) in tokens)
            {
                placeholders.Add(
                    new EmailPlaceholderDto
                    {
                        Key = key,
                        Value = value,
                        CreatedBy = createdBy
                    });
            }

            return placeholders;
        }
    }
}