using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerFeedbackService : IEmployerFeedbackService
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<EmployerFeedbackService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;

        public EmployerFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<EmployerFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityRepository = opportunityRepository;
        }

        public async Task<int> SendEmployerFeedbackEmailsAsync(string userName)
        {
            var referralDate = _dateTimeProvider.GetReferralDateAsync(GetBankHolidays, _configuration.EmployerFeedbackTimeSpan);

            var emailsSent = 0;
            if (referralDate != null)
            {
                emailsSent = await SendEmployerFeedbackEmailsAsync(referralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendEmployerFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var referrals = await _opportunityRepository.GetReferralsForEmployerFeedbackAsync(referralDate);

            try
            {
                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "employer_contact_name", referral.EmployerContact.ToTitleCase() },
                    };

                    await SendEmail(EmailTemplateName.EmployerFeedback, referral.OpportunityId, referral.EmployerContactEmail,
                        "Your industry placement progress – ESFA", tokens, userName);
                }

                await SetEmployerFeedbackAsSent(referrals.Select(r => r.OpportunityId), userName);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private async Task SetEmployerFeedbackAsSent(IEnumerable<int> opportunityIds, string userName)
        {
            var itemsToBeCompleted = opportunityIds.Select(id => new UsernameForFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<Opportunity>>(itemsToBeCompleted);

            await _opportunityRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.EmployerFeedbackSentOn,
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
                tokens);

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
