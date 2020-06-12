using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService
{
    public class When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_And_Status_Is_Processing
        : IClassFixture<ProviderQuarterlyUpdateEmailFixture>
    {
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;
        private readonly int _result;

        public When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_And_Status_Is_Processing(ProviderQuarterlyUpdateEmailFixture testFixture)
        {
            _emailService = Substitute.For<IEmailService>();
            
            var messageQueueService = Substitute.For<IMessageQueueService>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var backgroundProcessHistory = new BackgroundProcessHistoryBuilder().Build();
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Processing.ToString();

            _backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();
            _backgroundProcessHistoryRepository
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>())
                .Returns(backgroundProcessHistory);

            var providerQuarterlyUpdateEmailService = new Application.Services.ProviderQuarterlyUpdateEmailService(
                testFixture.Logger,
                    _emailService,
                    _providerRepository, _backgroundProcessHistoryRepository,
                    messageQueueService, testFixture.DateTimeProvider);

            _result = providerQuarterlyUpdateEmailService
                .SendProviderQuarterlyUpdateEmailsAsync(1, "TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetProvidersWithFundingAsync_Is_Not_Called()
        {
            _providerRepository
                .DidNotReceive()
                .GetProvidersWithFundingAsync();
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _backgroundProcessHistoryRepository
                .Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>());
        }
        
        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Is_Not_Called()
        {
            _backgroundProcessHistoryRepository
                .DidNotReceiveWithAnyArgs()
                .UpdateAsync(Arg.Any<BackgroundProcessHistory>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Not_Called()
        {
            _emailService
                .DidNotReceiveWithAnyArgs()
                .SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(0);
        }
    }
}
