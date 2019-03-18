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
    public class ReferralService : IReferralService
    {
        //public const string EmployerReferralEmailTemplateName = "EmployerReferral";
        public const string ProviderReferralEmailTemplateName = "ProviderReferral";
        public const string ProviderReferralEmailSubject = "Industry Placement Matching Referral";
        public const string ReplyToAddress = "DummyAddressToBeOverriddebByService"; //reply address is currently ignored by DAS Notifications

        private readonly ILogger<ReferralService> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public ReferralService(IEmailService emailService,
            IRepository<EmailHistory> emailHistoryRepository,
            IRepository<EmailTemplate> emailTemplateRepository,
            IRepository<Opportunity> opportunityRepository,
            IMapper mapper,
            ILogger<ReferralService> logger)
        {
            _emailService = emailService;
            _emailTemplateRepository = emailTemplateRepository;
            _emailHistoryRepository = emailHistoryRepository;
            _opportunityRepository = (IOpportunityRepository)opportunityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task SendEmployerEmail(int opportunityId)
        {
            throw new NotImplementedException();
            /* Note: can set up list of providers into a single placeholder using code like this:
            var sb = new StringBuilder();
            sb.AppendLine("# This is a heading");
            sb.AppendLine("This is some text.");
            sb.AppendLine("* bullet 1");
            sb.AppendLine("* bullet 2");
            sb.AppendLine("---");
            sb.Append("\r\n");
            sb.Append("Some final text\r\n");
            sb.Append("...");
            sb.Append("and a footer");

            Will be something like:
            # [Provider name]
            [Venue postcode]
            Contact name: [provider primary contact name]
            Telephone: [provider primary contact number]
            Email: [provider primary contact email]
            Has students learning:
            * [shortened qual title 1]
            * [shortened qual title 2]
            * Etc


            var placeholders = new List<EmailPlaceholderDto>
            {
                new EmailPlaceholderDto {Key = "providers_list", Value = sb.ToString()}
            };
            */
        }

        public async Task SendProviderReferralEmail(int opportunityId)
        {
            var emailTemplate = await GetEmailTemplate(ProviderReferralEmailTemplateName);

            var referrals = await GetOpportunityReferrals(opportunityId);

            foreach (var referral in referrals)
            {
                var toAddress = referral.ProviderPrimaryContactEmail;

                var numberOfPlacements = referral.PlacementsKnown.GetValueOrDefault()
                    ? referral.Placements.ToString()
                    : "at least one";

                var placeholders = new List<EmailPlaceholderDto>
                {
                    new EmailPlaceholderDto { Key = "primary_contact_name", Value = referral.ProviderPrimaryContact },
                    new EmailPlaceholderDto { Key = "provider_name", Value = referral.ProviderName },
                    new EmailPlaceholderDto { Key = "route", Value = referral.RouteName },
                    new EmailPlaceholderDto { Key = "venue_postcode", Value = referral.ProviderVenuePostcode },
                    new EmailPlaceholderDto { Key = "distance", Value = referral.SearchRadius.ToString() },
                    new EmailPlaceholderDto { Key = "job_role", Value = referral.JobTitle },
                    new EmailPlaceholderDto { Key = "employer_business_name", Value = referral.EmployerName},
                    new EmailPlaceholderDto { Key = "employer_contact_name ", Value = referral.EmployerContact },
                    new EmailPlaceholderDto { Key = "employer_contact_number", Value = referral.EmployerContactPhone },
                    new EmailPlaceholderDto { Key = "employer_contact_email ", Value = referral.EmployerContactEmail },
                    new EmailPlaceholderDto { Key = "employer_postcode", Value = referral.Postcode },
                    new EmailPlaceholderDto { Key = "number_of_placements", Value = numberOfPlacements }
                };

                var tokens = ConvertPlaceholdersToTokens(placeholders);

                await _emailService.SendEmail(emailTemplate.TemplateName,
                    toAddress,
                    ProviderReferralEmailSubject,
                    tokens,
                    ReplyToAddress);

                await SaveEmailHistory(emailTemplate, placeholders, opportunityId, toAddress);
            }
        }

        private async Task SaveEmailHistory(EmailTemplate emailTemplate, List<EmailPlaceholderDto> placeholders, int opportunityId, string emailAddress)
        {
            var emailPlaceholders = _mapper.Map<IList<EmailPlaceholder>>(placeholders);
            _logger.LogInformation($"Saving {emailPlaceholders.Count} {nameof(EmailPlaceholder)} items.");

            var emailHistory = new EmailHistory
            {
                OpportunityId = opportunityId,
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

        private async Task<IList<OpportunityReferralDto>> GetOpportunityReferrals(int opportunityId)
        {
            var opportunityReferralDto = await _opportunityRepository.GetProviderOpportunities(opportunityId);
            return opportunityReferralDto;
        }

        private IDictionary<string, string> ConvertPlaceholdersToTokens(IEnumerable<EmailPlaceholderDto> placeholders)
        {
            var tokens = new Dictionary<string, string>();

            foreach (var placeholder in placeholders)
            {
                tokens[placeholder.Key] = placeholder.Value;
            }

            return tokens;
        }
    }


}
