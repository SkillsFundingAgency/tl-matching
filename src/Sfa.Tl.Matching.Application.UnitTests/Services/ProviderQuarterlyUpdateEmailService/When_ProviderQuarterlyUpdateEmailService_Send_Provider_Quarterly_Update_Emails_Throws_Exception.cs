using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService
{
    public class When_ProviderQuarterlyUpdateEmailService_Send_Provider_Quarterly_Update_Emails_Throws_Exception
        : IClassFixture<ProviderQuarterlyUpdateEmailFixture>
    {
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;
        private readonly IList<BackgroundProcessHistory> _receivedProviderQuarterlyUpdateEmailRequestHistories;
        private readonly ILogger<Application.Services.ProviderQuarterlyUpdateEmailService> _logger;
        private readonly int _result;

        public When_ProviderQuarterlyUpdateEmailService_Send_Provider_Quarterly_Update_Emails_Throws_Exception(ProviderQuarterlyUpdateEmailFixture testFixture)
        {
            _emailService = Substitute.For<IEmailService>();
            
            var messageQueueService = Substitute.For<IMessageQueueService>();
            _logger = Substitute.For<ILogger<Application.Services.ProviderQuarterlyUpdateEmailService>>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            _backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();
            _backgroundProcessHistoryRepository
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>())
                .Returns(new BackgroundProcessHistoryBuilder().Build());
            
            _receivedProviderQuarterlyUpdateEmailRequestHistories = new List<BackgroundProcessHistory>();
            _backgroundProcessHistoryRepository
                .UpdateAsync(Arg.Do<BackgroundProcessHistory>
                (x => _receivedProviderQuarterlyUpdateEmailRequestHistories.Add(
                    new BackgroundProcessHistory
                    {
                        Id = x.Id,
                        RecordCount = x.RecordCount,
                        Status = x.Status,
                        StatusMessage = x.StatusMessage,
                        ModifiedOn = x.ModifiedOn,
                        ModifiedBy = x.ModifiedBy
                    }
                )));

            _emailService
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>())
                .Throws(new Exception());

            var providerQuarterlyUpdateEmailService = new Application.Services.ProviderQuarterlyUpdateEmailService(
                testFixture.Configuration, _logger, 
                    _emailService,
                    _providerRepository, _backgroundProcessHistoryRepository,
                    messageQueueService, testFixture.DateTimeProvider);

            _result = providerQuarterlyUpdateEmailService
                .SendProviderQuarterlyUpdateEmailsAsync(1, "TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetProvidersWithFundingAsync_Is_Called_Exactly_Once()
        {
            _providerRepository
                .Received(1)
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
        public void Then_BackgroundProcessHistoryRepository_Update_Is_Called_Exactly_Twice()
        {
            _backgroundProcessHistoryRepository
                .ReceivedWithAnyArgs(2)
                .UpdateAsync(Arg.Any<BackgroundProcessHistory>());
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Is_Called_With_Expected_Values()
        {
            //Can't check Status and count here because NSubstitute only remembers the last one
            _backgroundProcessHistoryRepository
                .Received()
                .UpdateAsync(Arg.Is<BackgroundProcessHistory>(
                    p => p.Id == 1
                         //&& p.RecordCount == 1
                         && p.ModifiedBy == "TestUser"
                ));
        }
        
        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Sets_Expected_Values_In_First_Call()
        {
            var history = _receivedProviderQuarterlyUpdateEmailRequestHistories.First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(BackgroundProcessHistoryStatus.Processing.ToString());
            history.StatusMessage.Should().BeNullOrEmpty();
            history.RecordCount.Should().Be(1);
            history.ModifiedBy.Should().Be("TestUser");
        }
        
        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Sets_Expected_Values_In_Second_Call()
        {
            var history = _receivedProviderQuarterlyUpdateEmailRequestHistories.Skip(1).First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(BackgroundProcessHistoryStatus.Error.ToString());
            history.StatusMessage.Should().NotBeNullOrEmpty();
            history.RecordCount.Should().Be(0);
            history.ModifiedBy.Should().Be("TestUser");
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_Logger_Is_Called_With_Error_Message()
        {
            _logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o =>
                    o.ToString()
                        .Contains(
                            "Error sending provider quarterly update emails.")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(0);
        }
    }
}
