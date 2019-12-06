using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerFeedbackService : IEmployerFeedbackService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IRepository<Employer> _employerRepository;
        private readonly ILogger<EmployerFeedbackService> _logger;

        public EmployerFeedbackService(
            MatchingConfiguration configuration,
            ILogger<EmployerFeedbackService> logger,
            IEmailService emailService,
            IRepository<Employer> employerRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _employerRepository = employerRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendEmployerFeedbackEmailsAsync(string userName)
        {
            return 0;
        }
    }
}