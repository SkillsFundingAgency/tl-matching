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
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly ILogger<ProviderFeedbackService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;
        private readonly IMessageQueueService _messageQueueService;

        public ProviderFeedbackService(IEmailService emailService,
            IRepository<Provider> providerRepository,
            IRepository<ProviderFeedbackRequestHistory> providerFeedbackRequestHistoryRepository,
            IMessageQueueService messageQueueService,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            ILogger<ProviderFeedbackService> logger)
        {
            _emailService = emailService;
            _providerRepository = providerRepository;
            _providerFeedbackRequestHistoryRepository = providerFeedbackRequestHistoryRepository;
            _messageQueueService = messageQueueService;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _logger = logger;
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

            var providers = await ((IProviderRepository)_providerRepository).GetProvidersWithFundingAsync();

            foreach (var provider in providers)
            {
                var toAddress = provider.PrimaryContactEmail;

                var tokens = new Dictionary<string, string>
                {
                    { "provider_name", provider.Name },
                    { "primary_contact_name", provider.PrimaryContact},
                    { "primary_contact_email", provider.PrimaryContactEmail },
                    { "primary_contact_phone", provider.PrimaryContactPhone }
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

                await _emailService.SendEmail(EmailTemplateName.ProviderQuarterlyUpdate.ToString(),
                    toAddress,
                    "Industry Placement Matching Provider Update",
                    tokens,
                    "");
            }

            providerFeedbackRequestHistory.ProviderCount = providers.Count;
            providerFeedbackRequestHistory.Status = (short)ProviderFeedbackRequestStatus.Sent;
            providerFeedbackRequestHistory.ModifiedBy = userName;
            providerFeedbackRequestHistory.ModifiedOn = _dateTimeProvider.UtcNow();
            await _providerFeedbackRequestHistoryRepository.Update(providerFeedbackRequestHistory);
        }
    }
}