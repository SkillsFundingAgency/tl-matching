using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public ProviderFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository
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
            _opportunityItemRepository = opportunityItemRepository;
        }

        public async Task<int> SendProviderFeedbackEmailsAsync(string userName)
        {
            var referralDate = await GetReferralDateAsync();

            var emailsSent = 0;
            if (referralDate != null)
            {
                emailsSent = await SendProviderFeedbackEmailsAsync(referralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<DateTime?> GetReferralDateAsync()
        {
            var employerFeedbackTimespan = TimeSpan.Parse(_configuration.EmployerFeedbackTimeSpan);
            var bankHolidays = await _bankHolidayRepository.GetMany(
                    d => d.Date <= DateTime.Today)
                .Select(d => d.Date)
                .OrderBy(d => d.Date)
                .ToListAsync();

            if (_dateTimeProvider.IsHoliday(_dateTimeProvider.UtcNow().Date, bankHolidays))
                return null;

            var referralDate = _dateTimeProvider
                .AddWorkingDays(
                    _dateTimeProvider.UtcNow().Date,
                    employerFeedbackTimespan,
                    bankHolidays);

            return referralDate;
        }

        private async Task<int> SendProviderFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var data = await _opportunityRepository.GetAllReferralsForProviderFeedbackAsync(referralDate);
            var referrals = _opportunityRepository.GetDistinctReferralsForProviderFeedbackAsync(data);

            try
            {
                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.Displayname },
                        { "company_name", referral.Companyname},
                    };

                    await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                        referral.ProviderPrimaryContactEmail, "Your industry placement progress – ESFA", tokens,
                        userName);

                    if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail))
                        await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                            referral.ProviderSecondaryContactEmail, "Your industry placement progress – ESFA", tokens,
                            userName);
                }

                await SetOpportunityItemsProviderFeedbackAsSent(data.Select(r => r.OpportunityItemId),userName);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider feedback emails. {ex.Message} ";
                _logger.LogError(ex, errorMessage);

                throw;
            }
        }

        private async Task SetOpportunityItemsProviderFeedbackAsSent(IEnumerable<int> opportunityItemIds, string userName)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemWithUsernameForProviderFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.ProviderFeedbackSent,
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

    }

    public interface IProviderFeedbackService
    {
        Task<int> SendProviderFeedbackEmailsAsync(string userName);
    }
}
