using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public ProviderFeedbackService(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<Provider> providerRepository,
            IRepository<ProviderFeedbackRequestHistory> providerFeedbackRequestHistoryRepository,
            IMessageQueueService messageQueueService,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _providerRepository = providerRepository;
            _providerFeedbackRequestHistoryRepository = providerFeedbackRequestHistoryRepository;
            _messageQueueService = messageQueueService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task RequestProviderQuarterlyUpdateAsync(string userName)
        {
            var providerFeedbackRequestHistoryId = await _providerFeedbackRequestHistoryRepository.Create(
                new ProviderFeedbackRequestHistory
                {
                    Status = ProviderFeedbackRequestStatus.Pending.ToString(),
                    CreatedBy = userName
                });

            await _messageQueueService.PushProviderQuarterlyRequestMessageAsync(new SendProviderFeedbackEmail
            {
                ProviderFeedbackRequestHistoryId = providerFeedbackRequestHistoryId
            });
        }

        public async Task SendProviderQuarterlyUpdateEmailsAsync(int providerFeedbackRequestHistoryId, string userName)
        {
            var providerFeedbackRequestHistory =
                await _providerFeedbackRequestHistoryRepository
                    .GetSingleOrDefault(p => p.Id == providerFeedbackRequestHistoryId);

            if (providerFeedbackRequestHistory == null ||
                providerFeedbackRequestHistory.Status != ProviderFeedbackRequestStatus.Pending.ToString())
            {
                return;
            }

            var numberOfProviderEmailsSent = 0;

            try
            {
                var providers = await ((IProviderRepository)_providerRepository).GetProvidersWithFundingAsync();

                await UpdateProviderFeedbackRequestHistory(providerFeedbackRequestHistory,
                    providers.Count,
                    ProviderFeedbackRequestStatus.Processing,
                    userName);

                foreach (var provider in providers)
                {
                    var toAddress = provider.PrimaryContactEmail;
					var hasVenues = provider.ProviderVenues.Any();

                    var tokens = new Dictionary<string, string>
                    {
                        { "provider_name", provider.Name },
                        { "primary_contact_name", provider.PrimaryContact },
                        { "primary_contact_email", provider.PrimaryContactEmail },
                        { "primary_contact_phone", provider.PrimaryContactPhone },
                        { "provider_has_venues", hasVenues ? "yes" : "no" },
                        { "provider_has_no_venues", hasVenues ? "no" : "yes" }
                    };

                    var venuesListBuilder = new StringBuilder();

                    foreach (var providerVenue in provider.ProviderVenues)
                    {
                        venuesListBuilder.AppendLine($"{providerVenue.Postcode}:");
                        if (!providerVenue.Qualifications.Any())
                        {
                            venuesListBuilder.AppendLine("* no qualifications with an industry placement option");
                        }

                        foreach (var qualification in providerVenue.Qualifications
                                                                   .OrderBy(q => q.LarsId))
                        {
                            venuesListBuilder.AppendLine($"* {qualification.LarsId}: {qualification.Title}");
                        }

                        venuesListBuilder.AppendLine("");
                    }

                    tokens.Add("venues_and_qualifications_list", venuesListBuilder.ToString());

                    var secondaryDetailsBuilder = new StringBuilder();
                    if (!string.IsNullOrEmpty(provider.SecondaryContact))
                    {
                        secondaryDetailsBuilder.AppendLine(
                            $"We also have the following secondary contact for {provider.Name}:");
                        secondaryDetailsBuilder.AppendLine($"* Name: {provider.SecondaryContact}");
                        secondaryDetailsBuilder.AppendLine($"* Email: {provider.SecondaryContactEmail}");
                        secondaryDetailsBuilder.AppendLine($"* Telephone: {provider.SecondaryContactPhone}");
                    }

                    tokens.Add("secondary_contact_details", secondaryDetailsBuilder.ToString());

                    await SendEmail(EmailTemplateName.ProviderQuarterlyUpdate, null, toAddress,
                        "Industry Placement Matching Provider Update", tokens, userName);

                    numberOfProviderEmailsSent++;
                }

                await UpdateProviderFeedbackRequestHistory(providerFeedbackRequestHistory,
                    numberOfProviderEmailsSent,
                    ProviderFeedbackRequestStatus.Complete,
                    userName);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider quarterly update emails. {ex.Message} " +
                                   $"Provider feedback id {providerFeedbackRequestHistory.Id}";

                _logger.LogError(ex, errorMessage);

                await UpdateProviderFeedbackRequestHistory(providerFeedbackRequestHistory,
                    numberOfProviderEmailsSent,
                    ProviderFeedbackRequestStatus.Error,
                    userName,
                    errorMessage);
            }
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

        private async Task UpdateProviderFeedbackRequestHistory(
            ProviderFeedbackRequestHistory providerFeedbackRequestHistory,
            int providerCount, ProviderFeedbackRequestStatus status,
            string userName, string errorMessage = null)
        {
            providerFeedbackRequestHistory.ProviderCount = providerCount;
            providerFeedbackRequestHistory.Status = status.ToString();
            providerFeedbackRequestHistory.StatusMessage = errorMessage;
            providerFeedbackRequestHistory.ModifiedBy = userName;
            providerFeedbackRequestHistory.ModifiedOn = _dateTimeProvider.UtcNow();
            await _providerFeedbackRequestHistoryRepository.Update(providerFeedbackRequestHistory);
        }
    }
}