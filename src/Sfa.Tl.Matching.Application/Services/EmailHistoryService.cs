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
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailHistoryService(IRepository<EmailHistory> emailHistoryRepository,
            IRepository<EmailTemplate> emailTemplateRepository,
            IMapper mapper,
            ILogger<EmailHistoryService> logger)
        {
            _emailHistoryRepository = emailHistoryRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task SaveEmailHistory(string emailTemplateName, IDictionary<string, string> tokens, int? opportunityId, string emailAddress, string createdBy)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == emailTemplateName);

            var placeholders = ConvertTokensToEmailPlaceholderDtos(tokens, createdBy);

            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);
            _logger.LogInformation($"Saving {emailPlaceholders.Count} {nameof(EmailPlaceholder)} items.");

            var emailHistory = new EmailHistory
            {
                OpportunityId = opportunityId,
                EmailTemplateId = emailTemplate.Id,
                EmailPlaceholder = emailPlaceholders,
                SentTo = emailAddress,
                CreatedBy = createdBy
            };
            await _emailHistoryRepository.Create(emailHistory);
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
