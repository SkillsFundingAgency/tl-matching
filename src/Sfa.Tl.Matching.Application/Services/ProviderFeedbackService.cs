using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public ProviderFeedbackService(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IEmailService emailService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityRepository = opportunityRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendProviderFeedbackEmailsAsync(string userName)
        {
            try
            {
                var numberOfEmailsSent = 0;

                if (!IsNthWorkingDay(_configuration.ProviderFeedbackWorkingDayInMonth))
                {
                    _logger.LogInformation(
                        "Provider feedback service exited because today is not a valid day for processing.");
                    return numberOfEmailsSent;
                }

                var previousMonthDate = _dateTimeProvider.UtcNow().AddMonths(-1);
                var previousMonth = previousMonthDate.ToString("MMMM");
                var referrals = await _opportunityRepository.GetReferralsForProviderFeedbackAsync(previousMonthDate);

                var referralsGroupedByProvider = referrals
                    .GroupBy(r => r.ProviderId)
                    .ToDictionary(r => r.Key, r => r.ToList());

                foreach (var (_, value) in referralsGroupedByProvider)
                {
                    var provider = value.First();

                    var tokens = CreateTokens(value, previousMonth);

                    var secondaryContactEmailDetails = new StringBuilder();

                    if (!string.IsNullOrWhiteSpace(provider.SecondaryContactEmail)
                        && provider.SecondaryContactEmail != provider.PrimaryContactEmail)
                    {
                        secondaryContactEmailDetails.Append("We also sent this email to ");
                        secondaryContactEmailDetails.Append($"{provider.SecondaryContactEmail} ");
                        secondaryContactEmailDetails.Append(
                            $"who we have as {provider.ProviderDisplayName}’s secondary contact for industry placements. ");
                        secondaryContactEmailDetails.AppendLine("Please coordinate your response with them.");
                    }

                    tokens["other_email_details"] = secondaryContactEmailDetails.ToString();

                    numberOfEmailsSent += await SendEmailsAsync(provider.PrimaryContactEmail, tokens, userName);

                    if (!string.IsNullOrWhiteSpace(provider.SecondaryContactEmail)
                        && provider.SecondaryContactEmail != provider.PrimaryContactEmail)
                    {
                        var primaryContactEmailDetails = new StringBuilder();
                        primaryContactEmailDetails.Append("We also sent this email to ");
                        primaryContactEmailDetails.Append($"{provider.PrimaryContact} ");
                        primaryContactEmailDetails.Append(
                            $"who we have as {provider.ProviderDisplayName}’s primary contact for industry placements. ");
                        primaryContactEmailDetails.AppendLine("Please coordinate your response with them.");

                        tokens["contact_name"] = provider.SecondaryContact;
                        tokens["other_email_details"] = primaryContactEmailDetails.ToString();

                        numberOfEmailsSent += await SendEmailsAsync(provider.SecondaryContactEmail, tokens, userName);
                    }
                }

                return numberOfEmailsSent;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private static IDictionary<string, string> CreateTokens(
            IReadOnlyCollection<ProviderFeedbackDto> providerFeedbackDtos,
            string previousMonth)
        {
            var provider = providerFeedbackDtos.First();
            var employersList = new StringBuilder();

            var employers = providerFeedbackDtos
                .GroupBy(p => p.EmployerCompanyName)
                .ToDictionary(p => p.Key, p => p.OrderByDescending(e => e.EmployerCompanyName)
                    .ToList());

            foreach (var employer in employers.OrderBy(e => e.Key))
            {
                var companyName = employer.Key;
                employersList.AppendLine($"* {companyName}");

                var hasFirstRouteBeenShown = false;
                foreach (var venue in employer.Value)
                {
                    foreach (var route in venue.Routes.OrderBy(r => r))
                    {
                        if (hasFirstRouteBeenShown)
                        {
                            employersList.Append("and ");
                        }

                        hasFirstRouteBeenShown = true;
                        employersList.AppendLine(
                            $"for students studying {route.ToLower()} courses at {venue.Town} {venue.Postcode}");
                    }
                }

                employersList.AppendLine("");
            }

            var contact = provider.SecondaryContactEmail == provider.PrimaryContactEmail
                      && !string.IsNullOrWhiteSpace(provider.SecondaryContact)
                      && provider.SecondaryContact != provider.PrimaryContact
                ? $"{provider.PrimaryContact} or {provider.SecondaryContact}"
                : provider.PrimaryContact;

            var otherEmailDetails = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(provider.SecondaryContactEmail)
                && provider.SecondaryContactEmail != provider.PrimaryContactEmail)
            {
                otherEmailDetails.Append("We also sent this email to ");
                otherEmailDetails.Append($"{provider.SecondaryContactEmail} ");
                otherEmailDetails.Append(
                    $"who we have as {provider.ProviderDisplayName}’s secondary contact for industry placements. ");
                otherEmailDetails.AppendLine("Please coordinate your response with them.");
            }

            var tokens = new Dictionary<string, string>
            {
                {"contact_name", contact},
                {"previous_month", previousMonth},
                {"provider_name", provider.ProviderDisplayName},
                {"employers_list", employersList.ToString()},
                {"other_email_details", otherEmailDetails.ToString()},
            };

            return tokens;
        }

        private async Task<int> SendEmailsAsync(string toAddress, IDictionary<string, string> tokens, string userName)
        {
            await _emailService.SendEmailAsync(null, null,
                EmailTemplateName.ProviderFeedbackV2.ToString(),
                toAddress,
                tokens,
                userName);

            return 1;
        }

        public bool IsNthWorkingDay(int workingDay)
        {
            var today = _dateTimeProvider.UtcNow().Date;
            var holidays = _bankHolidayRepository
                .GetManyAsync(h => h.Date.Month == today.Month)
                .Select(h => h.Date)
                .ToList();

            return today == _dateTimeProvider.GetNthWorkingDayDate(today, workingDay, holidays);
        }
    }
}
