using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
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

        private const string ErrorSummary = "There was an error sending an email from the industry placement matching service.";
        private const string ErrorUnknownSummary = "We cannot determine whether or not the following email was sent.";

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

            var emailDeliveryStatusPayLoad = JsonConvert.DeserializeObject<EmailDeliveryStatusPayLoad>(payload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

            var updatedCount = await _emailService.UpdateEmailStatus(emailDeliveryStatusPayLoad);

            if (emailDeliveryStatusPayLoad.EmailDeliveryStatus.ToUpper() != "DELIVERED" && updatedCount != -1)
                await PushEmailDeliveryStatusAsync(emailDeliveryStatusPayLoad.id);

            return updatedCount;
        }

        public async Task SendEmailDeliveryStatusAsync(Guid notificationId)
        {
            var emailHistoryDto = await _emailService.GetEmailHistoryAsync(notificationId);
            if (emailHistoryDto == null)
            {
                _logger.LogInformation($"Notification Id={notificationId} cannot be found in EmailHistory table");
                return;
            }

            var failedEmailDto = await _emailService.GetFailedEmailAsync(notificationId);
            if (failedEmailDto == null)
            {
                _logger.LogInformation($"Notification Id={notificationId} cannot be found in Notify");
                return;
            }

            var summary = ErrorSummary;
            if (emailHistoryDto.Status == "unknown-failure")
            {
                summary = ErrorUnknownSummary;
                if (emailHistoryDto.Status != failedEmailDto.Status)
                {
                    await _emailService.UpdateEmailStatus(new EmailDeliveryStatusPayLoad
                    {
                        id = notificationId,
                        status = failedEmailDto.Status
                    });
                }
            }

            Enum.TryParse(emailHistoryDto.EmailTemplateName, out EmailTemplateName emailTemplateName);

            var emailBody = await GetEmailBody(emailTemplateName, emailHistoryDto);

            var tokens = new Dictionary<string, string>
            {
                { "summary", summary },
                { "email_type", emailTemplateName.Humanize().ToLower() },
                { "body", emailBody },
                { "reason", failedEmailDto.FailedEmailType.Humanize() },
                { "sender_username", emailHistoryDto.CreatedBy },
                { "failed_email_body", failedEmailDto.Body }
            };

            await _emailService.SendEmailAsync(emailHistoryDto.OpportunityId, EmailTemplateName.FailedEmailV2.ToString(),
                _configuration.MatchingServiceSupportEmailAddress, tokens, "System");
        }

        private async Task<string> GetEmailBody(EmailTemplateName emailTemplateName, EmailHistoryDto emailHistoryDto)
        {
            var emailBody = string.Empty;
            var emailBodyDto = await GetEmailBodyDto(emailTemplateName, emailHistoryDto);

            if (emailBodyDto != null)
                emailBody = GetEmailBodyFrom(emailBodyDto);

            return emailBody;
        }

        private async Task<EmailBodyDto> GetEmailBodyDto(EmailTemplateName emailTemplateName, EmailHistoryDto emailHistoryDto)
        {
            if (!emailHistoryDto.OpportunityId.HasValue) 
                return null;

            EmailBodyDto emailBodyDto = null;

            switch (emailTemplateName)
            {
                case EmailTemplateName.EmployerReferral:
                case EmailTemplateName.EmployerFeedback:
                case EmailTemplateName.EmployerReferralComplex:
                case EmailTemplateName.EmployerReferralV3:
                    emailBodyDto = await _opportunityRepository.GetFailedEmployerEmailAsync(
                        emailHistoryDto.OpportunityId.Value,
                        emailHistoryDto.SentTo);
                    break;
                case EmailTemplateName.ProviderReferral:
                case EmailTemplateName.ProviderQuarterlyUpdate:
                case EmailTemplateName.ProviderReferralComplex:
                case EmailTemplateName.ProviderReferralV3:
                case EmailTemplateName.ProviderFeedback:
                    emailBodyDto = await _opportunityRepository.GetFailedProviderEmailAsync(
                        emailHistoryDto.OpportunityId.Value,
                        emailHistoryDto.SentTo);
                    break;
            }

            return emailBodyDto;
        }

        private async Task PushEmailDeliveryStatusAsync(Guid notificationId)
        {
            await _messageQueueService.PushFailedEmailMessageAsync(new SendFailedEmail
            {
                NotificationId = notificationId
            });
        }

        private static string GetEmailBodyFrom(EmailBodyDto dto)
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