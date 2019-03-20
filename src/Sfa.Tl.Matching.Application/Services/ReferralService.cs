using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public const string EmployerReferralEmailTemplateName = "EmployerReferral";
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

        public async Task SendEmployerEmail(int opportunityId)
        {

            //TODO:

            //Test in AT
            //See comments in https://github.com/SkillsFundingAgency/tl-matching/pull/75

            //you could use DidNotReceived for hen_NotificationsApi_SendEmail_Is_Not_Called()

            //Email subject and replyto - if not in use please remove
            //DefaultReplyToAddress and ReplyToAddress - remove - if this is not working just remove it for now

            // Do we need to log here that we didn't find any suitable template?
            //if (emailTemplate == null)


            // await _referralService.SendProviderReferralEmail(id);
            //This need to happen before we redirect to Email Sent Screen i.e. in the post of CheckAnswers
            //Also you might want to move this call to Opportunity service

            //Use GetNumberOfPlacements everywhere

            var emailTemplate = await GetEmailTemplate(EmployerReferralEmailTemplateName);

            var referrals = await GetEmployerReferrals(opportunityId);

            var employer = referrals.FirstOrDefault();

            if (employer == null)
            {
                return;
            }

            var toAddress = employer.EmployerContactEmail;

            var numberOfPlacements = GetNumberOfPlacements(employer.PlacementsKnown, employer.Placements);

            var tokens = new Dictionary<string, string>
            {
                { "employer_contact_name", employer.EmployerContact },
                { "employer_business_name", employer.EmployerName },
                { "employer_contact_number", employer.EmployerContactPhone },
                { "employer_contact_email", employer.EmployerContactEmail },
                { "employer_postcode", employer.Postcode },
                { "number_of_placements", numberOfPlacements },
                { "route", employer.RouteName },
                { "job_role", employer.JobTitle }
            };

            var sb = new StringBuilder();

            foreach (var referral in referrals)
            {
                sb.AppendLine($"# {referral.ProviderName}");
                sb.AppendLine($"{referral.ProviderVenuePostcode}");
                sb.AppendLine($"Contact name: {referral.ProviderPrimaryContact}");
                //sb.AppendLine($"Telephone: {referral.ProviderPrimaryContactPhone}");
                sb.AppendLine($"Email: {referral.ProviderPrimaryContactEmail}");
                sb.AppendLine("Has students learning: ");

                foreach (var qualificationShortTitle in referral.QualificationShortTitles)
                {
                    sb.AppendLine($"* {qualificationShortTitle}");
                }
                sb.AppendLine(""); //Need a blank line, otherwise the next heading won't be formatted
            }

            tokens.Add("providers_list", sb.ToString());

            await _emailService.SendEmail(emailTemplate.TemplateName,
                toAddress,
                "Industry Placement Matching Referral",
                tokens,
                "mike.wild@digital.education.gov.uk");

            await SaveEmailHistory(emailTemplate, tokens, opportunityId, toAddress, employer.CreatedBy);
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least one";
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
                
                var tokens = new Dictionary<string, string>
                {
                    { "primary_contact_name", referral.ProviderPrimaryContact },
                    { "provider_name", referral.ProviderName },
                    { "route", referral.RouteName },
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

                await _emailService.SendEmail(emailTemplate.TemplateName,
                    toAddress,
                    ProviderReferralEmailSubject,
                    tokens,
                    ReplyToAddress);

                await SaveEmailHistory(emailTemplate, tokens, opportunityId, toAddress, referral.CreatedBy);
            }
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

        private async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            var emailTemplate = await _emailTemplateRepository.GetSingleOrDefault(t => t.TemplateName == templateName);
            return emailTemplate;
        }

        private async Task<IList<EmployerReferralDto>> GetEmployerReferrals(int opportunityId)
        {
            var employerReferralDto = await _opportunityRepository.GetEmployerReferrals(opportunityId);
            return employerReferralDto;
        }

        private async Task<IList<OpportunityReferralDto>> GetOpportunityReferrals(int opportunityId)
        {
            var opportunityReferralDto = await _opportunityRepository.GetProviderOpportunities(opportunityId);
            return opportunityReferralDto;
        }

        private IEnumerable<EmailPlaceholderDto> ConvertTokensToEmailPlaceholderDtos(IDictionary<string, string> tokens, string createdBy)
        {
            var placeholders = new List<EmailPlaceholderDto>();

            foreach (var token in tokens)
            {
                placeholders.Add(
                    new EmailPlaceholderDto
                    {
                        Key = token.Key,
                        Value = token.Value,
                        CreatedBy = createdBy
                    });
            }

            return placeholders;
        }
    }
}
