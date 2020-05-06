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
                await PushEmailDeliveryStatusAsync(emailDeliveryStatusPayLoad.Id);

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

            var emailBodyDto = await _emailService.GetEmailBodyFromNotifyClientAsync(notificationId);
            if (emailBodyDto == null)
            {
                _logger.LogInformation($"Notification Id={notificationId} cannot be found in Notify");
                return;
            }

            var summary = ErrorSummary;
            if (emailHistoryDto.Status == "unknown-failure")
            {
                summary = ErrorUnknownSummary;
                if (emailHistoryDto.Status != emailBodyDto.Status)
                {
                    await _emailService.UpdateEmailStatus(new EmailDeliveryStatusPayLoad
                    {
                        Id = notificationId,
                        Status = emailBodyDto.Status
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
                { "reason", emailBodyDto.EmailDeliveryStatusType.Humanize() },
                { "sender_username", emailHistoryDto.CreatedBy },
                { "email_body", emailBodyDto.Body }
            };

            await _emailService.SendEmailAsync(emailHistoryDto.OpportunityId, EmailTemplateName.EmailDeliveryStatus.ToString(),
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
                case EmailTemplateName.EmployerFeedback:
                case EmailTemplateName.EmployerFeedbackV2:
                case EmailTemplateName.EmployerReferral:
                case EmailTemplateName.EmployerReferralComplex:
                case EmailTemplateName.EmployerReferralV3:
                case EmailTemplateName.EmployerReferralV4:
                case EmailTemplateName.EmployerReferralV5:
                    emailBodyDto = await _opportunityRepository.GetEmailDeliveryStatusForEmployerAsync(
                        emailHistoryDto.OpportunityId.Value,
                        emailHistoryDto.SentTo);
                    break;
                case EmailTemplateName.ProviderFeedback:
                case EmailTemplateName.ProviderFeedbackV2:
                case EmailTemplateName.ProviderQuarterlyUpdate:
                case EmailTemplateName.ProviderReferral:
                case EmailTemplateName.ProviderReferralComplex:
                case EmailTemplateName.ProviderReferralV3:
                case EmailTemplateName.ProviderReferralV4:
                case EmailTemplateName.ProviderReferralV5:
                    emailBodyDto = await _opportunityRepository.GetEmailDeliveryStatusForProviderAsync(
                        emailHistoryDto.OpportunityId.Value,
                        emailHistoryDto.SentTo);
                    break;
            }

            return emailBodyDto;
        }

        private async Task PushEmailDeliveryStatusAsync(Guid notificationId)
        {
            await _messageQueueService.PushEmailDeliveryStatusMessageAsync(new SendEmailDeliveryStatus
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