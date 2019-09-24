using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services.FeedbackFactory
{
    public abstract class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public abstract Task<int> SendFeedbackEmailsAsync(string userName);

        protected FeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IRepository<OpportunityItem> opportunityItemRepository
        )
        {
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityItemRepository = opportunityItemRepository;
        }

        public async Task<DateTime?> GetReferralDateAsync()
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

        public async Task SetOpportunityItemsProviderFeedbackAsSent(IEnumerable<int> opportunityItemIds, string userName)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemWithUsernameForProviderFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.ProviderFeedbackSent,
                x => x.ModifiedBy);
        }
        public async Task SetOpportunityItemsEmployerFeedbackAsSent(IEnumerable<int> opportunityItemIds, string userName)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemWithUsernameForEmployerFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.EmployerFeedbackSent,
                x => x.ModifiedBy);
        }

        public async Task SendEmail(EmailTemplateName template, int? opportunityId,
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
