using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class FailedEmailService : IFailedEmailService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<FailedEmailService> _logger;

        public FailedEmailService(MatchingConfiguration configuration,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IOpportunityRepository opportunityRepository,
            ILogger<FailedEmailService> logger)
        {
            _configuration = configuration;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _opportunityRepository = opportunityRepository;
            _logger = logger;
        }

        public async Task SendFailedEmailAsync(SendFailedEmail failedEmailData)
        {
            var failedEmailDto = await _emailService.GetFailedEmailAsync(failedEmailData.NotificationId);
            var emailHistoryDto = await _emailHistoryService.GetEmailHistoryAsync(new Guid(failedEmailData.NotificationId));
            var emailTemplateName = (EmailTemplateName)emailHistoryDto.EmailTemplateId;

            if (!emailHistoryDto.OpportunityId.HasValue)
            {
                _logger.LogInformation($"Notification Id={failedEmailData.NotificationId} does not have an opportunity id");
                return;
            }

            var emailBodyDto = await _opportunityRepository.GetFailedOpportunityEmailAsync(emailHistoryDto.OpportunityId.Value,
                emailHistoryDto.SentTo);

            var tokens = new Dictionary<string, string>
                {
                    { "email_type", emailTemplateName.Humanize() },
                    { "body", GetFailedEmailBody(emailBodyDto) },
                    { "reason", failedEmailDto.Reason },
                    { "sender_username", emailHistoryDto.CreatedBy },
                    { "failed_email_body", failedEmailDto.Body }
                };

            await _emailService.SendEmailAsync(null, EmailTemplateName.FailedEmail.ToString(),
                _configuration.MatchingServiceSupportEmailAddress, tokens, "System");
        }

        private static string GetFailedEmailBody(FailedEmailBodyDto dto)
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