using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
        public const string ProvisionGapReportEmailTemplateName = "ProvisionGapReport";
        public const string ProviderReferralEmailSubject = "Industry Placement Matching Referral";
        public const string ProvisionGapReportEmailSubject = "Industry Placement Matching Provision Gap Report";
        public const string ReplyToAddress = "reply@test.com";

        private readonly ILogger<ReferralService> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailPlaceholder> _emailPlaceholderRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<Opportunity> _opportunityRepository;

        public ReferralService(IEmailService emailService,
            IRepository<EmailHistory> emailHistoryRepository,
            IRepository<EmailPlaceholder> emailPlaceholderRepository,
            IRepository<EmailTemplate> emailTemplateRepository,
            IRepository<Opportunity> opportunityRepository,
            IMapper mapper,
            ILogger<ReferralService> logger)
        {
            _emailService = emailService;
            _emailTemplateRepository = emailTemplateRepository;
            _emailHistoryRepository = emailHistoryRepository;
            _emailPlaceholderRepository = emailPlaceholderRepository;
            _opportunityRepository = opportunityRepository;
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

            var templateName = ProviderReferralEmailTemplateName;

            var emailTemplate = await GetEmailTemplate(templateName);
            var opportunity = await GetOpportunity(opportunityId);

            //TODO: This should be a loop over referrals#
            var referral = opportunity.Referral.First();

            var providerEmailAddress = "";

            var placeholders = new List<EmailPlaceholderDto>
                {
                    new EmailPlaceholderDto { Key = "primary_contact_name", Value = "Mike" },
                    new EmailPlaceholderDto { Key = "provider_name", Value = "Your Provider" },
                    new EmailPlaceholderDto { Key = "route", Value = opportunity.Route?.Name },
                    new EmailPlaceholderDto { Key = "venue_postcode", Value = "AA1 1AA" },
                    new EmailPlaceholderDto { Key = "distance", Value = opportunity.Distance.ToString() },
                    new EmailPlaceholderDto { Key = "job_role", Value = "Assistant Chef" },
                    new EmailPlaceholderDto { Key = "employer_business_name", Value = "Big Co." },
                    new EmailPlaceholderDto { Key = "employer_contact_name ", Value = "Bog Boss" },
                    new EmailPlaceholderDto { Key = "employer_contact_number", Value = "0201 234 567" },
                    new EmailPlaceholderDto { Key = "employer_contact_email ", Value = "test@test.com" },
                    new EmailPlaceholderDto { Key = "employer_postcode", Value = "XX1 2YY" },
                    new EmailPlaceholderDto { Key = "number_of_placements", Value = "at least 1" }
                };

            var tokens = ConvertPlaceholdersToTokens(placeholders);

            _emailService.SendEmail(templateName,
                providerEmailAddress,
                ProviderReferralEmailSubject,
                tokens,
                ReplyToAddress);
            
            await SaveEmailHistory(emailTemplate, placeholders, opportunity, providerEmailAddress);
        }

        public async Task SendProvisionGapEmail(int opportunityId)
        {
            var templateName = ProvisionGapReportEmailSubject;
            var emailTemplate = await GetEmailTemplate(templateName);
            var opportunity = await GetOpportunity(opportunityId);

            var placeholders = new List<EmailPlaceholderDto>
            {
            };

            var tokens = ConvertPlaceholdersToTokens(placeholders);
            var providerEmailAddress = "";

            _emailService.SendEmail(templateName,
                providerEmailAddress,
                ProviderReferralEmailSubject,
                tokens,
                ReplyToAddress);

            await SaveEmailHistory(emailTemplate, placeholders, opportunity, providerEmailAddress);
        }

        private async Task SaveEmailHistory(EmailTemplate emailTemplate, List<EmailPlaceholderDto> placeholders, Opportunity opportunity, string emailAddress)
        {
            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);
            _logger.LogInformation($"Saving {emailPlaceholders.Count} { nameof(EmailPlaceholder) }.");

            //await _emailPlaceholderRepository.CreateMany(emailPlaceholders);

            var emailHistory = new EmailHistory
            {
                OpportunityId = opportunity.Id,
                EmailTemplateId = emailTemplate.Id,
                EmailPlaceholder = emailPlaceholders,
                SentTo = emailAddress,
            };
            await _emailHistoryRepository.Create(emailHistory);
        }

        private async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == templateName);
            return emailTemplate;
        }

        private async Task<Opportunity> GetOpportunity(int opportunityId)
        {
            var opportunity = await _opportunityRepository
                .GetSingleOrDefault(
                    t => t.Id == opportunityId,
                    opp => opp.ProvisionGap,
                    opp => opp.Referral,
                    opp => opp.Route);
            return opportunity;
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
