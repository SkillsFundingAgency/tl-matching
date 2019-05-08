using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public ProviderFeedbackService(
            ILogger<ProviderFeedbackService> logger,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<Provider> providerRepository,
            IRepository<ProviderFeedbackRequestHistory> providerFeedbackRequestHistoryRepository,
            IMessageQueueService messageQueueService,
            IDateTimeProvider dateTimeProvider)
        {
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
                    Status = (short)ProviderFeedbackRequestStatus.Pending,
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
                providerFeedbackRequestHistory.Status != (int)ProviderFeedbackRequestStatus.Pending)
            {
                return;
            }

            var emailTemplateName = EmailTemplateName.ProviderQuarterlyUpdate.ToString();
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

                    var tokens = new Dictionary<string, string>
                    {
                        {"provider_name", provider.Name},
                        {"primary_contact_name", provider.PrimaryContact},
                        {"primary_contact_email", provider.PrimaryContactEmail},
                        {"primary_contact_phone", provider.PrimaryContactPhone}
                    };

                    var venuesListBuilder = new StringBuilder();
                    foreach (var providerVenue in provider.ProviderVenues)
                    {
                        venuesListBuilder.AppendLine($"{providerVenue.Postcode}:");
                        foreach (var qualification in providerVenue.Qualifications)
                        {
                            venuesListBuilder.AppendLine($"* {qualification.LarsId}: {qualification.ShortTitle}");
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

                    await _emailService.SendEmail(emailTemplateName,
                        toAddress,
                        "Industry Placement Matching Provider Update",
                        tokens,
                        "");

                    await _emailHistoryService.SaveEmailHistory(emailTemplateName,
                        tokens,
                        null,
                        toAddress,
                        userName);

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

        private async Task UpdateProviderFeedbackRequestHistory(
            ProviderFeedbackRequestHistory providerFeedbackRequestHistory,
            int providerCount, ProviderFeedbackRequestStatus status,
            string userName, string errorMessage = null)
        {
            providerFeedbackRequestHistory.ProviderCount = providerCount;
            providerFeedbackRequestHistory.Status = (short)status;
            providerFeedbackRequestHistory.StatusMessage = errorMessage;
            providerFeedbackRequestHistory.ModifiedBy = userName;
            providerFeedbackRequestHistory.ModifiedOn = _dateTimeProvider.UtcNow();
            await _providerFeedbackRequestHistoryRepository.Update(providerFeedbackRequestHistory);
        }
    }
}