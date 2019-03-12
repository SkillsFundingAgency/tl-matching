using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Provider_Email
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailPlaceholder> _emailPlaceholderRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public When_ReferralService_Is_Called_To_Send_Provider_Email()
        {
            _configuration = new MatchingConfiguration();
            _notificationsApi = Substitute.For<INotificationsApi>();
            _emailService = Substitute.For<IEmailService>();

            var logger = Substitute.For<ILogger<ReferralService>>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            _emailPlaceholderRepository = Substitute.For<IRepository<EmailPlaceholder>>();
            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();

            var referralService = new ReferralService(_emailService, 
                _emailHistoryRepository,_emailPlaceholderRepository,_emailTemplateRepository, 
                mapper, logger);

            referralService.SendProviderEmail(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<dynamic>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailPlaceholderRepository_CreateMany_Is_Called_Exactly_Once()
        {
            _emailPlaceholderRepository
                .Received(1)
                .CreateMany(Arg.Any<IList<Domain.Models.EmailPlaceholder>>());
        }
    }
}
