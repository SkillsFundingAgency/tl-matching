using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
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
            var sb = new StringBuilder();

            if (employerReferral == null) return;

            var tokens = new Dictionary<string, string>
            {
                { "employer_contact_name", employerReferral.EmployerContact },
                { "employer_business_name", employerReferral.CompanyName },
                { "employer_contact_number", employerReferral.EmployerContactPhone },
                { "employer_contact_email", employerReferral.EmployerContactEmail },
                { "employer_postcode", employerReferral.Postcode }
            };

            foreach (var data in employerReferral.WorkplaceDetails.OrderBy(dto => dto.WorkplaceTown))
            {
                var placements = GetNumberOfPlacements(data.PlacementsKnown, data.Placements);
                var providers = string.Join(", ", data.ProviderDetails.Select(dto => dto.ProviderName));

                sb.AppendLine($"# {data.WorkplaceTown} {data.WorkplacePostcode}");
                sb.AppendLine($"*Job role: {data.JobRole}");
                sb.AppendLine($"*Students wanted: {placements}");
                sb.AppendLine($"*Providers selected: {providers}");
                sb.AppendLine("");
            }

            tokens.Add("placements_list", sb.ToString());

            await SendEmail(EmailTemplateName.EmployerReferralComplex, opportunityId, employerReferral.EmployerContactEmail,
                "Your industry placement referral – ESFA National Apprenticeship Service", tokens, employerReferral.CreatedBy);
        }

        public async Task SendProviderReferralEmail(int opportunityId)
        {
            var referrals = await GetOpportunityReferrals(opportunityId);

            foreach (var referral in referrals)
            {
                var toAddress = referral.ProviderPrimaryContactEmail;
                var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                var tokens = new Dictionary<string, string>
                {
                    { "primary_contact_name", referral.ProviderPrimaryContact },
                    { "provider_name", referral.ProviderName },
                    { "route", referral.RouteName.ToLowerInvariant() },
                    { "venue_postcode", $"{referral.ProviderVenueTown} {referral.ProviderVenuePostcode}" },
                    { "search_radius", referral.DistanceFromEmployer },
                    { "job_role", referral.JobRole },
                    { "employer_business_name", referral.CompanyName },
                    { "employer_contact_name", referral.EmployerContact},
                    { "employer_contact_number", referral.EmployerContactPhone },
                    { "employer_contact_email", referral.EmployerContactEmail },
                    { "employer_postcode", $"{referral.Town} {referral.Postcode }" },
                    { "number_of_placements", placements }
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
                : "at least 1";
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
