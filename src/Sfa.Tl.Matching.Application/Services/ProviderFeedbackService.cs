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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _providerRepository;

        public ProviderFeedbackService(IEmailService emailService,
            IRepository<Provider> providerRepository,
            IMapper mapper,
            ILogger<ProviderFeedbackService> logger)
        {
            _emailService = emailService;
            _providerRepository = providerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SendProviderQuarterlyUpdateEmailAsync()
        {
            var providers = await ((IProviderRepository)_providerRepository).GetProvidersWithFundingAsync();
            //foreach create tokens and send email

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

                /*venues_and_qualifications_list - foreach
                {
                    sb.AppendLine("[provider venue postcode 1]:");
                    sb.AppendLine("* [LAR ID]: [qualification title 1]");
                    sb.AppendLine("* [LAR ID]: [qualification title 2]");
                    sb.AppendLine("");
                }
                */
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

                //TODO: Move more code into emailservice, call with template name enum.
                //       this should look up template, send, and update history
                await _emailService.SendEmail(EmailTemplateName.ProviderQuarterlyUpdate.ToString(),
                    toAddress,
                    "Industry Placement Matching Provider Update",
                    tokens,
                    "");
            }
        }
    }
}
