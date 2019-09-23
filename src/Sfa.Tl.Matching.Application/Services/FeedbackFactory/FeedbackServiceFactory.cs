using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Services.FeedbackFactory
{
    public class FeedbackServiceFactory<T> : IFeedbackServiceFactory<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public FeedbackServiceFactory(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<T> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository
        )
        {
            _logger = logger;
            _opportunityRepository = opportunityRepository;
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityItemRepository = opportunityItemRepository;
        }

        public IFeedbackService CreateInstanceOf(FeedbackEmailTypes emailTypes)
        {
            switch (emailTypes)
            {
                case FeedbackEmailTypes.EmployerFeedback:
                    return new EmployerFeedbackService(_mapper, _configuration, _logger as ILogger<EmployerFeedbackService>, _dateTimeProvider,
                        _emailService,
                        _emailHistoryService, _bankHolidayRepository, _opportunityRepository,
                        _opportunityItemRepository);

                case FeedbackEmailTypes.ProviderFeedback:
                    return new ProviderFeedbackService(_mapper, _configuration, _logger as ILogger<ProviderFeedbackService>, _dateTimeProvider,
                        _emailService,
                        _emailHistoryService, _bankHolidayRepository, _opportunityRepository,
                        _opportunityItemRepository);

                default:
                    throw new ArgumentOutOfRangeException(nameof(emailTypes), emailTypes, null);
            }
        }
    }

    public enum FeedbackEmailTypes
    {
        EmployerFeedback,
        ProviderFeedback
    }
}