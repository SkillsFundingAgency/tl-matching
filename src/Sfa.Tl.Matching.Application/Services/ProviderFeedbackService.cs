using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<ProviderFeedbackService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<Provider> _providerRepository;

        public ProviderFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<Provider> providerRepository
            )
        {
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityRepository = opportunityRepository;
            _providerRepository = providerRepository;
        }

        public async Task<int> SendProviderFeedbackEmailsAsync(string userName)
        {
            var referralDate =
                _dateTimeProvider.GetReferralDateAsync(GetBankHolidays, _configuration.EmployerFeedbackTimeSpan);

            var emailsSent = 0;
            if (referralDate != null)
            {
                emailsSent = await SendProviderFeedbackEmailsAsync(referralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendProviderFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var referrals = await _opportunityRepository.GetAllReferralsForProviderFeedbackAsync(referralDate);

            try
            {
                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.Displayname },
                        { "company_name", referral.Companyname}
                    };

                    await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                        referral.ProviderPrimaryContactEmail, "Your industry placement progress – ESFA", tokens,
                        userName);

                    if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail))
                        await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                            referral.ProviderSecondaryContactEmail, "Your industry placement progress – ESFA", tokens,
                            userName);
                }

                await SetProviderFeedbackSentOnDate(referrals.Select(r => r.ProviderId), userName);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider feedback emails. {ex.Message} ";
                _logger.LogError(ex, errorMessage);

                throw;
            }
        }

        private async Task SetProviderFeedbackSentOnDate(IEnumerable<int> providerIds, string userName)
        {
            var itemsToBeCompleted = providerIds.Select(id => new UsernameForFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<Provider>>(itemsToBeCompleted);

            await _providerRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.ProviderFeedbackSentOn,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
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

        private List<DateTime> GetBankHolidays => _bankHolidayRepository.GetMany(d => d.Date <= DateTime.Today)
            .Select(d => d.Date)
            .OrderBy(d => d.Date)
            .ToList();

    }
}
