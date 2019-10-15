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
    public class EmployerFeedbackService : FeedbackService
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployerFeedbackService> _logger;
        
        public EmployerFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<EmployerFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository) 
            : base(configuration, emailService, dateTimeProvider, bankHolidayRepository, backgroundProcessHistoryRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _opportunityRepository = opportunityRepository;
        }

        public override async Task<int> SendFeedbackEmailsAsync(string userName)
        {
            var emailsSent = 0;
            if (ReferralDate != null)
            {
                emailsSent = await SendEmployerFeedbackEmailsAsync(ReferralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendEmployerFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var referrals = await _opportunityRepository.GetReferralsForEmployerFeedbackAsync(referralDate);

            try
            {
                var historyId = await CreateBackgroundProcessHistoryAsync(BackgroundProcessType.EmployerFeedbackEmail);

                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "employer_contact_name", referral.EmployerContact.ToTitleCase() },
                    };

                    await SendEmailAsync(EmailTemplateName.EmployerFeedback, referral.OpportunityId, referral.EmployerContactEmail, tokens, userName);
                }

                await SetEmployerFeedbackAsSentAsync(referrals.Select(r => r.OpportunityId), userName);

                await UpdateBackgroundProcessHistoryAync(historyId, referrals.Count);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private async Task SetEmployerFeedbackAsSentAsync(IEnumerable<int> opportunityIds, string userName)
        {
            var itemsToBeCompleted = opportunityIds.Select(id => new UsernameForFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<Opportunity>>(itemsToBeCompleted);

            await _opportunityRepository.UpdateManyWithSpecifedColumnsOnlyAsync(updates,
                x => x.EmployerFeedbackSentOn,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }
    }
}
