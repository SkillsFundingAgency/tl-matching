using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly ILogger<ReferralService> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<Opportunity> _opportunityRepository;

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
            _opportunityRepository = opportunityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SendEmployerReferralEmail(int opportunityId)
        {
            var emailTemplate = await GetEmailTemplate(EmailTemplateName.EmployerReferral.ToString());

            var employerReferral = await GetEmployerReferrals(opportunityId);

            if (employerReferral == null)
            {
                return;
            }

            var toAddress = employerReferral.EmployerContactEmail;

            var numberOfPlacements = GetNumberOfPlacements(employerReferral.PlacementsKnown, employerReferral.Placements);

            var tokens = new Dictionary<string, string>
            {
                { "employer_contact_name", employerReferral.EmployerContact },
                { "employer_business_name", employerReferral.EmployerName },
                { "employer_contact_number", employerReferral.EmployerContactPhone },
                { "employer_contact_email", employerReferral.EmployerContactEmail },
                { "employer_postcode", employerReferral.Postcode },
                { "number_of_placements", numberOfPlacements },
                { "route", employerReferral.RouteName.ToLowerInvariant() },
                { "job_role", employerReferral.JobTitle }
            };

            var sb = new StringBuilder();

            foreach (var providerReferral in employerReferral.ProviderReferralInfo)
            {
                sb.AppendLine($"# {providerReferral.ProviderName}");
                sb.AppendLine($"{providerReferral.ProviderVenuePostcode}");
                sb.AppendLine($"Contact name: {providerReferral.ProviderPrimaryContact}");
                sb.AppendLine($"Telephone: {providerReferral.ProviderPrimaryContactPhone}");
                sb.AppendLine($"Email: {providerReferral.ProviderPrimaryContactEmail}");
                sb.AppendLine("");
                sb.AppendLine("Has students learning: ");

                foreach (var qualificationShortTitle in providerReferral.QualificationShortTitles)
                {
                    sb.AppendLine($"* {qualificationShortTitle}");
                }
                sb.AppendLine(""); //Need a blank line, otherwise the next heading won't be formatted
            }

            tokens.Add("providers_list", sb.ToString());

            await _emailService.SendEmail(EmailTemplateName.EmployerReferral.ToString(),
                toAddress,
                "Industry Placement Matching Referral",
                tokens,
                "");

            await SaveEmailHistory(emailTemplate, tokens, opportunityId, toAddress, employerReferral.CreatedBy);
        }

        public async Task SendProviderReferralEmail(int opportunityId)
        {
            var emailTemplate = await GetEmailTemplate(EmailTemplateName.ProviderReferral.ToString());

            var referrals = await GetOpportunityReferrals(opportunityId);

            foreach (var referral in referrals)
            {
                var toAddress = referral.ProviderPrimaryContactEmail;

                var numberOfPlacements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);
                
                var tokens = new Dictionary<string, string>
                {
                    { "primary_contact_name", referral.ProviderPrimaryContact },
                    { "provider_name", referral.ProviderName },
                    { "route", referral.RouteName.ToLowerInvariant() },
                    { "venue_postcode", referral.ProviderVenuePostcode },
                    { "search_radius", referral.SearchRadius.ToString() },
                    { "job_role", referral.JobTitle },
                    { "employer_business_name", referral.EmployerName },
                    { "employer_contact_name", referral.EmployerContact},
                    { "employer_contact_number", referral.EmployerContactPhone },
                    { "employer_contact_email", referral.EmployerContactEmail },
                    { "employer_postcode", referral.Postcode },
                    { "number_of_placements", numberOfPlacements }
                };

                await _emailService.SendEmail(EmailTemplateName.ProviderReferral.ToString(),
                    toAddress,
                    "Industry Placement Matching Referral",
                    tokens,
                    "");

                await SaveEmailHistory(emailTemplate, tokens, opportunityId, toAddress, referral.CreatedBy);
            }
        }

        private async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            return await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == templateName);
        }

        private async Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId)
        {
            return await ((IOpportunityRepository)_opportunityRepository).GetEmployerReferrals(opportunityId);
        }

        private async Task<IList<OpportunityReferralDto>> GetOpportunityReferrals(int opportunityId)
        {
            return await ((IOpportunityRepository)_opportunityRepository).GetProviderOpportunities(opportunityId);
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least one";
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

        private async Task SaveEmailHistory(EmailTemplate emailTemplate, IDictionary<string, string> tokens, int opportunityId, string emailAddress, string createdBy)
        {
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
    }
}
