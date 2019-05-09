using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<Opportunity> _opportunityRepository;

        public ReferralService(
            MatchingConfiguration configuration,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<Opportunity> opportunityRepository)
        {
            _configuration = configuration;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _opportunityRepository = opportunityRepository;
        }

        public async Task SendEmployerReferralEmail(int opportunityId)
        {
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

            await SendEmail(EmailTemplateName.EmployerReferral, opportunityId, toAddress,
                "Industry Placement Matching Referral", tokens, employerReferral.CreatedBy);
        }

        public async Task SendProviderReferralEmail(int opportunityId)
        {
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

                await SendEmail(EmailTemplateName.ProviderReferral, opportunityId, toAddress,
                    "Industry Placement Matching Referral", tokens, referral.CreatedBy);
            }
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

        private async Task SendEmail(EmailTemplateName template, int? opportunityId, 
            string toAddress, string subject, 
            IDictionary<string, string> tokens, string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            await _emailService.SendEmail(template.ToString(),
                    toAddress,
                    subject,
                    tokens,
                    "");

            await _emailHistoryService.SaveEmailHistory(template.ToString(),
                tokens,
                opportunityId,
                toAddress,
                createdBy);
        }
    }
}
