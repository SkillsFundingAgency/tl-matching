﻿using System;
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
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IMapper _mapper;

        public EmailService(MatchingConfiguration configuration,
            IAsyncNotificationClient notificationClient,
            IRepository<EmailTemplate> emailTemplateRepository,
            IRepository<EmailHistory> emailHistoryRepository,
            IRepository<FunctionLog> functionLogRepository,
            IMapper mapper,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
            _emailHistoryRepository = emailHistoryRepository;
            _functionLogRepository = functionLogRepository;
            _mapper = mapper;
            _notificationClient = notificationClient;
            _logger = logger;
        }

        public async Task SendEmailAsync(string templateName,
            string toAddress, int? opportunityId, int? opportunityItemId,
            IDictionary<string, string> personalisationTokens, string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefaultAsync(t => t.TemplateName == templateName);
            if (emailTemplate == null)
            {
                var message = $"Email template {templateName} not found. No emails sent.";
                _logger.LogWarning(message);
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = message,
                    FunctionName = nameof(EmailService),
                    RowNumber = -1
                });

                return;
            }

            _logger.LogInformation($"Sending {templateName} email to {toAddress}");

            await SendEmailAndSaveHistoryAsync(opportunityId, opportunityItemId, toAddress, emailTemplate, personalisationTokens, createdBy);
        }
        
        public async Task<EmailDeliveryStatusDto> GetEmailBodyFromNotifyClientAsync(Guid notificationId)
        {
            var notification = await _notificationClient.GetNotificationByIdAsync(notificationId.ToString());
            var dto = _mapper.Map<EmailDeliveryStatusDto>(notification);

            return dto;
        }

        public async Task<EmailHistoryDto> GetEmailHistoryAsync(Guid notificationId)
        {
            var emailHistory = await _emailHistoryRepository.GetSingleOrDefaultAsync(eh => eh.NotificationId == notificationId,
                navigationPropertyPath: eh => eh.EmailTemplate);

            var dto = _mapper.Map<EmailHistoryDto>(emailHistory);

            return dto;
        }

        public async Task<int> UpdateEmailStatus(EmailDeliveryStatusPayLoad payLoad)
        {
            var data = await _emailHistoryRepository.GetFirstOrDefaultAsync(history =>
                history.NotificationId == payLoad.Id);

            if (data == null) return -1;

            data.Status = payLoad.Status;
            data.ModifiedOn = DateTime.UtcNow;
            data.ModifiedBy = "System";

            await _emailHistoryRepository.UpdateWithSpecifiedColumnsOnlyAsync(data,
                history => history.Status,
                history => history.ModifiedOn,
                history => history.ModifiedBy);

            return 1;
        }

        private async Task SendEmailAndSaveHistoryAsync(int? opportunityId, int? opportunityItemId, string recipient, EmailTemplate emailTemplate,
            IDictionary<string, string> personalisationTokens, string createdBy)
        {
            try
            {
                var tokens = personalisationTokens.Select(x => new { key = x.Key, val = (dynamic)x.Value })
                    .ToDictionary(item => item.key, item => item.val);

                var emailresponse = await _notificationClient.SendEmailAsync(recipient, emailTemplate.TemplateId, tokens);
                
                Guid.TryParse(emailresponse.id, out var notificationId);

                await SaveEmailHistoryAsync(notificationId, emailTemplate.Id, personalisationTokens, opportunityId, opportunityItemId,
                    recipient, createdBy);
            }
            catch (Exception ex)
            {
                var message = $"Error sending email template {emailTemplate?.TemplateId} to {recipient}. {ex.Message}";
                _logger.LogError(ex, message);

                message += $"\r\nInner exception: {ex.InnerException?.Message}\r\n" +
                           $"Stack trace: {ex.StackTrace}";
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = message,
                    FunctionName = nameof(EmailService),
                    RowNumber = -1
                });
            }
        }

        private async Task SaveEmailHistoryAsync(Guid notificatonId, int emailTemplateId,
            IDictionary<string, string> tokens,
            int? opportunityId, int? opportunityItemId, string emailAddress, string createdBy)
        {
            var placeholders = ConvertTokensToEmailPlaceholderDtos(tokens, createdBy);

            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);

            _logger.LogInformation($"Saving {emailPlaceholders.Count} {nameof(EmailPlaceholder)} items.");

            var emailHistory = new EmailHistory
            {
                NotificationId = notificatonId,
                OpportunityId = opportunityId,
                OpportunityItemId = opportunityItemId,
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