using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderQuarterlyUpdateEmailService : IProviderQuarterlyUpdateEmailService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<ProviderQuarterlyUpdateEmailService> _logger;

        public ProviderQuarterlyUpdateEmailService(
            MatchingConfiguration configuration,
            ILogger<ProviderQuarterlyUpdateEmailService> logger,
            IEmailService emailService,
            IProviderRepository providerRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
            IMessageQueueService messageQueueService,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _providerRepository = providerRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
            _messageQueueService = messageQueueService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task RequestProviderQuarterlyUpdateAsync(string userName)
        {
            var backgroundProcessHistoryId = await _backgroundProcessHistoryRepository.CreateAsync(
                new BackgroundProcessHistory
                {
                    ProcessType = BackgroundProcessType.ProviderFeedbackRequest.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString(),
                    CreatedBy = userName
                });

            await _messageQueueService.PushProviderQuarterlyRequestMessageAsync(new SendProviderFeedbackEmail
            {
                BackgroundProcessHistoryId = backgroundProcessHistoryId
            });
        }

        public async Task<int> SendProviderQuarterlyUpdateEmailsAsync(int backgroundProcessHistoryId, string userName)
        {
            var backgroundProcessHistory =
                await _backgroundProcessHistoryRepository
                    .GetSingleOrDefaultAsync(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null ||
                backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString())
            {
                return 0;
            }

            var numberOfProviderEmailsSent = 0;

            try
            {
                var providers = await _providerRepository.GetProvidersWithFundingAsync();

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistory,
                    providers.Count,
                    BackgroundProcessHistoryStatus.Processing,
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
                                                                   .OrderBy(q => q.LarId))
                        {
                            venuesListBuilder.AppendLine($"* {qualification.LarId}: {qualification.Title}");
                        }

                        venuesListBuilder.AppendLine("");
                    }

                    tokens.Add("venues_and_qualifications_list", venuesListBuilder.ToString());

                    var secondaryDetailsBuilder = new StringBuilder();
                    if (!string.IsNullOrEmpty(provider.SecondaryContact))
                    {
                        secondaryDetailsBuilder.AppendLine($"We also have the following secondary contact for {provider.Name}:");
                        secondaryDetailsBuilder.AppendLine($"* Name: {provider.SecondaryContact}");
                        secondaryDetailsBuilder.AppendLine($"* Email: {provider.SecondaryContactEmail}");
                        secondaryDetailsBuilder.AppendLine($"* Telephone: {provider.SecondaryContactPhone}");
                    }

                    tokens.Add("secondary_contact_details", secondaryDetailsBuilder.ToString());

                    await SendEmailAsync(EmailTemplateName.ProviderQuarterlyUpdate, null, toAddress, tokens, userName);

                    numberOfProviderEmailsSent++;
                }

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistory,
                    numberOfProviderEmailsSent,
                    BackgroundProcessHistoryStatus.Complete,
                    userName);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider quarterly update emails. {ex.Message} " +
                                   $"Provider feedback id {backgroundProcessHistory.Id}";

                _logger.LogError(ex, errorMessage);

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistory,
                    numberOfProviderEmailsSent,
                    BackgroundProcessHistoryStatus.Error,
                    userName,
                    errorMessage);
            }

            return numberOfProviderEmailsSent;
        }

        private async Task SendEmailAsync(
            EmailTemplateName template, 
            int? opportunityId,
            string toAddress,
            IDictionary<string, string> tokens, 
            string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            await _emailService.SendEmailAsync(opportunityId, template.ToString(),
                    toAddress,
                    tokens, createdBy);
        }

        private async Task UpdateBackgroundProcessHistoryAsync(
            BackgroundProcessHistory backgroundProcessHistory,
            int providerCount, BackgroundProcessHistoryStatus historyStatus,
            string userName, string errorMessage = null)
        {
            backgroundProcessHistory.RecordCount = providerCount;
            backgroundProcessHistory.Status = historyStatus.ToString();
            backgroundProcessHistory.StatusMessage = errorMessage;
            backgroundProcessHistory.ModifiedBy = userName;
            backgroundProcessHistory.ModifiedOn = _dateTimeProvider.UtcNow();
            await _backgroundProcessHistoryRepository.UpdateAsync(backgroundProcessHistory);
        }
    }
}