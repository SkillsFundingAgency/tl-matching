using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailDeliveryStatusService : IEmailDeliveryStatusService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<EmailDeliveryStatusService> _logger;

        public EmailDeliveryStatusService(MatchingConfiguration configuration,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IMessageQueueService messageQueueService,
            ILogger<EmailDeliveryStatusService> logger)
        {
            _configuration = configuration;
            _emailService = emailService;
            _opportunityRepository = opportunityRepository;
            _messageQueueService = messageQueueService;
            _logger = logger;
        }

        public async Task<int> HandleEmailDeliveryStatusAsync(string payload)
        {
            if (string.IsNullOrEmpty(payload)) return -1;

            var callbackData = JsonConvert.DeserializeObject<EmailDeliveryStatusPayLoad>(payload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore
                });

            if (callbackData.status != "delivered")
                await PushEmailDeliveryStatusAsync(callbackData.id);

            return await _emailService.UpdateEmailStatus(callbackData);
        }

        public async Task SendEmailDeliveryStatusAsync(Guid notificationId)
        {
            var failedEmailDto = await _emailService.GetFailedEmailAsync(notificationId);
            var emailHistoryDto = await _emailService.GetEmailHistoryAsync(notificationId);

            Enum.TryParse(emailHistoryDto.EmailTemplateName, out EmailTemplateName emailTemplateName);

            if (!emailHistoryDto.OpportunityId.HasValue)
            {
                _logger.LogInformation($"Notification Id={notificationId} does not have an Opportunity Id");
                return;
            }

            var emailBodyDto = await _opportunityRepository.GetFailedOpportunityEmailAsync(emailHistoryDto.OpportunityId.Value,
                emailHistoryDto.SentTo);

            var tokens = new Dictionary<string, string>
            {
                { "email_type", emailTemplateName.Humanize() },
                { "body", GetEmailBody(emailBodyDto) },
                { "reason", failedEmailDto.FailedEmailType.Humanize() },
                { "sender_username", emailHistoryDto.CreatedBy },
                { "failed_email_body", failedEmailDto.Body }
            };

            await _emailService.SendEmailAsync(emailHistoryDto.OpportunityId.Value, EmailTemplateName.FailedEmail.ToString(),
                _configuration.MatchingServiceSupportEmailAddress, tokens, "System");
        }

        private async Task PushEmailDeliveryStatusAsync(Guid notificationId)
        {
            await _messageQueueService.PushFailedEmailMessageAsync(new SendFailedEmail
            {
                NotificationId = notificationId
            });
        }

        private static string GetEmailBody(EmailBodyDto dto)
        {
            var body = new StringBuilder();
            if (!string.IsNullOrEmpty(dto.ProviderName))
                body.AppendLine($"Provider name: {dto.ProviderName}");
            if (!string.IsNullOrEmpty(dto.PrimaryContactEmail))
                body.AppendLine($"Provider primary contact: {dto.PrimaryContactEmail}");
            if (!string.IsNullOrEmpty(dto.SecondaryContactEmail))
                body.AppendLine($"Provider secondary contact: {dto.SecondaryContactEmail}");
            if (!string.IsNullOrEmpty(dto.EmployerEmail))
                body.AppendLine($"Employer contact: {dto.EmployerEmail}");

            return body.ToString();
        }
    }
}