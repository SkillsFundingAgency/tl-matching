using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public abstract class FeedbackService : IFeedbackService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        protected FeedbackService(MatchingConfiguration configuration,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IDateTimeProvider dateTimeProvider,
            IRepository<BankHoliday> bankHolidayRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository
        )
        {
            _configuration = configuration;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _dateTimeProvider = dateTimeProvider;
            _bankHolidayRepository = bankHolidayRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }

        public abstract Task<int> SendFeedbackEmailsAsync(string userName);

        public async Task SendEmailAsync(EmailTemplateName template, int? opportunityId,
            string toAddress, IDictionary<string, string> tokens, string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            await _emailService.SendEmailAsync(opportunityId, template.ToString(),
                toAddress,
                tokens, createdBy);
            
        }

        public async Task<int> CreateBackgroundProcessHistoryAsync(BackgroundProcessType backgroundProcessType) =>
            await _backgroundProcessHistoryRepository.CreateAsync(
                new BackgroundProcessHistory
                {
                    ProcessType = backgroundProcessType.ToString(),
                    Status = BackgroundProcessHistoryStatus.Processing.ToString(),
                    CreatedBy = "System"
                });

        public async Task UpdateBackgroundProcessHistoryAync(int backgroundProcessHistoryId, int count)
        {
            var backgroundProcessHistory = await _backgroundProcessHistoryRepository.GetSingleOrDefaultAsync(p => p.Id == backgroundProcessHistoryId);

            backgroundProcessHistory.RecordCount = count;
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Complete.ToString();
            backgroundProcessHistory.ModifiedBy = "System";
            backgroundProcessHistory.ModifiedOn = _dateTimeProvider.UtcNow();

            await _backgroundProcessHistoryRepository.UpdateWithSpecifedColumnsOnlyAsync(backgroundProcessHistory,
                history => history.RecordCount,
                history => history.Status, 
                history => history.ModifiedBy, 
                history => history.ModifiedOn );
        }

        public DateTime? ReferralDate => _dateTimeProvider.GetReferralDateAsync(GetBankHolidays, _configuration.EmployerFeedbackTimeSpan);

        public List<DateTime> GetBankHolidays => _bankHolidayRepository.GetManyAsync(d => d.Date <= DateTime.Today)
            .Select(d => d.Date)
            .OrderBy(d => d.Date)
            .ToList();

    }
}