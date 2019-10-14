using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmailHistoryService : IEmailHistoryService
    {
        private readonly ILogger<EmailHistoryService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        
        public EmailHistoryService(IRepository<EmailHistory> emailHistoryRepository,
            IMapper mapper,
            ILogger<EmailHistoryService> logger)
        {
            _emailHistoryRepository = emailHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task SaveEmailHistoryAsync(Guid notificatonId, int emailTemplateId, IDictionary<string, string> tokens,
            int? opportunityId, string emailAddress, string createdBy)
        {
            var placeholders = ConvertTokensToEmailPlaceholderDtos(tokens, createdBy);

            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);
            _logger.LogInformation($"Saving {emailPlaceholders.Count} {nameof(EmailPlaceholder)} items.");

            var emailHistory = new EmailHistory
            {
                NotificationId = notificatonId,
                OpportunityId = opportunityId,
                EmailTemplateId = emailTemplateId,
                EmailPlaceholder = emailPlaceholders,
                SentTo = emailAddress,
                CreatedBy = createdBy
            };
            await _emailHistoryRepository.CreateAsync(emailHistory);
        }

        public async Task<EmailHistoryDto> GetEmailHistoryAsync(Guid notificationId)
        {
            var emailHistory = await _emailHistoryRepository.GetSingleOrDefaultAsync(eh => eh.NotificationId == notificationId);

            var dto = _mapper.Map<EmailHistoryDto>(emailHistory);

            return dto;
        }

        private IEnumerable<EmailPlaceholderDto> ConvertTokensToEmailPlaceholderDtos(IDictionary<string, string> tokens, string createdBy)
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