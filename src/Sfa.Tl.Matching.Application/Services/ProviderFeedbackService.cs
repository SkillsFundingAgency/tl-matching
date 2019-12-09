using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public ProviderFeedbackService(
            ILogger<ProviderFeedbackService> logger,
            IEmailService emailService,
            IProviderRepository providerRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _emailService = emailService;
            _providerRepository = providerRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendProviderFeedbackEmailsAsync(string userName)
        {
            return 0;
        }
    }
}
