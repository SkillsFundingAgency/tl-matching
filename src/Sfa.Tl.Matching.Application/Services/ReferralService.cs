using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        //public const string EmployerReferralEmailTemplateName = "EmployerReferral";
        public const string ProviderReferralEmailTemplateName = "ProviderReferral";
        //public const string ProvisionGapReportEmailTemplateName = "ProvisionGapReport";
        public const string ProviderReferralEmailSubject = "Industry Placement Matching Referral";
        public const string ReplyToAddress = "reply@test.com";

        private readonly ILogger<ReferralService> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailPlaceholder> _emailPlaceholderRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public ReferralService(IEmailService emailService,
            IRepository<EmailHistory> emailHistoryRepository,
            IRepository<EmailPlaceholder> emailPlaceholderRepository,
            IRepository<EmailTemplate> emailTemplateRepository,
            IMapper mapper,
            ILogger<ReferralService> logger)
        {
            _emailService = emailService;
            _emailTemplateRepository = emailTemplateRepository;
            _emailHistoryRepository = emailHistoryRepository;
            _emailPlaceholderRepository = emailPlaceholderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task SendEmployerEmail(int opportunityId)
        {
            throw new NotImplementedException();
        }

        public async Task SendProviderEmail(int opportunityId)
        {
            /*
              Get opportunity with referral or
              Get opportunity with providers

              Create list of placeholder dtos
              
              Pass placeholder dtos or convert to tokens

              Save placeholders and email history

              //TODO: What is ReplyToAddress?
            */

            var providerEmailAddress = "to@test.com";

            var placeholders = new List<EmailPlaceholderDto>
                {
                    new EmailPlaceholderDto { Key = "primary_contact_name", Value = "Mike" },
                    new EmailPlaceholderDto { Key = "provider_name", Value = "Your Provider" },
                    new EmailPlaceholderDto { Key = "route", Value = "Catering and hospitality" },
                    new EmailPlaceholderDto { Key = "venue_postcode", Value = "AA1 1AA" },
                    new EmailPlaceholderDto { Key = "distance", Value = "3.6" },
                    new EmailPlaceholderDto { Key = "job_role", Value = "Assistant Chef" },
                    new EmailPlaceholderDto { Key = "employer_business_name", Value = "Big Co." },
                    new EmailPlaceholderDto { Key = "employer_contact_name ", Value = "Bog Boss" },
                    new EmailPlaceholderDto { Key = "employer_contact_number", Value = "0201 234 567" },
                    new EmailPlaceholderDto { Key = "employer_contact_email ", Value = "test@test.com" },
                    new EmailPlaceholderDto { Key = "employer_postcode", Value = "XX1 2YY" },
                    new EmailPlaceholderDto { Key = "number_of_placements", Value = "at least 1" }
                };
            
            var tokens = ConvertPlaceholdersToTokens(placeholders);

            _emailService.SendEmail(ProviderReferralEmailTemplateName,
                providerEmailAddress,
                ProviderReferralEmailSubject,
                tokens,
                ReplyToAddress);

            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);
            _logger.LogInformation($"Saving { emailPlaceholders.Count } { nameof(EmailPlaceholder) }.");
            await _emailPlaceholderRepository.CreateMany(emailPlaceholders);
        }

        public Task SendProvisionGapEmail(int opportunityId)
        {
            throw new NotImplementedException();
        }

        private dynamic ConvertPlaceholdersToTokens(IEnumerable<EmailPlaceholderDto> placeholders)
        {
            IDictionary<string, object> tokens = new ExpandoObject();

            foreach (var placeholder in placeholders)
            {
                tokens[placeholder.Key] = placeholder.Value;
            }

            return tokens;
        }
    }
}
