using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IMapper _mapper;

        public EmailService(MatchingConfiguration configuration,
            IEmailHistoryService emailHistoryService,
            IAsyncNotificationClient notificationClient,
            IRepository<EmailTemplate> emailTemplateRepository,
            IMapper mapper,
            ILogger<EmailService> logger)
        {
            _emailHistoryService = emailHistoryService;
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

            _logger.LogInformation($"Sending {templateName} email to {toAddress}");

            await SendEmailViaNotificationsApiAndSaveHistoryAsync(opportunityId, toAddress, emailTemplate, personalisationTokens, createdBy);
        }
        
        public async Task<FailedEmailDto> GetFailedEmailAsync(Guid notificationId)
        {
            var notification = await _notificationClient.GetNotificationByIdAsync(notificationId.ToString());

            var dto = _mapper.Map<FailedEmailDto>(notification);

            return dto;
        }

        private async Task SendEmailViaNotificationsApiAndSaveHistoryAsync(int? opportunityId, string recipient, EmailTemplate emailTemplate,
            IDictionary<string, string> personalisationTokens, string createdBy)
        {
            try
            {
                var tokens = personalisationTokens.Select(x => new { key = x.Key, val = (dynamic)x.Value })
                    .ToDictionary(item => item.key, item => item.val);

                var emailresponse = await _notificationClient.SendEmailAsync(recipient, emailTemplate.TemplateId, tokens);

                Guid.TryParse(emailresponse.id, out var notificationId);

                await _emailHistoryService.SaveEmailHistoryAsync(notificationId, emailTemplate.Id, personalisationTokens, opportunityId,
                    recipient, createdBy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {emailTemplate.TemplateId} to {recipient}");
            }
        }
    }
}
