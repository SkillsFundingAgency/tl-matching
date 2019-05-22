using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Send_Provider_Quarterly_Update_Emails_Throws_Exception
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;
        private readonly IList<ProviderFeedbackRequestHistory> _receivedProviderFeedbackRequestHistories;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public When_ProviderFeedbackService_Send_Provider_Quarterly_Update_Emails_Throws_Exception(ProviderFeedbackFixture testFixture)
        {
            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var messageQueueService = Substitute.For<IMessageQueueService>();
            _logger = Substitute.For<ILogger<ProviderFeedbackService>>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            _providerFeedbackRequestHistoryRepository = Substitute.For<IRepository<ProviderFeedbackRequestHistory>>();
            _providerFeedbackRequestHistoryRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderFeedbackRequestHistory, bool>>>())
                .Returns(new ProviderFeedbackRequestHistoryBuilder().Build());
            
            _receivedProviderFeedbackRequestHistories = new List<ProviderFeedbackRequestHistory>();
            _providerFeedbackRequestHistoryRepository
                .Update(Arg.Do<ProviderFeedbackRequestHistory>
                (x => _receivedProviderFeedbackRequestHistories.Add(
                    new ProviderFeedbackRequestHistory
                    {
                        Id = x.Id,
                        ProviderCount = x.ProviderCount,
                        Status = x.Status,
                        StatusMessage = x.StatusMessage,
                        ModifiedOn = x.ModifiedOn,
                        ModifiedBy = x.ModifiedBy
                    }
                )));

            _emailService
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>())
                .Throws(new Exception());

            var providerFeedbackService = new ProviderFeedbackService(
                testFixture.Configuration, _logger, 
                    _emailService, _emailHistoryService,
                    _providerRepository, _providerFeedbackRequestHistoryRepository,
                    messageQueueService, testFixture.DateTimeProvider);

            providerFeedbackService
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
        public void Then_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerFeedbackRequestHistoryRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderFeedbackRequestHistory, bool>>>());
        }
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Is_Called_Exactly_Twice()
        {
            _providerFeedbackRequestHistoryRepository
                .ReceivedWithAnyArgs(2)
                .Update(Arg.Any<ProviderFeedbackRequestHistory>());
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Is_Called_With_Expected_Values()
        {
            //Can't check Status and count here because NSubstitute only remembers the last one
            _providerFeedbackRequestHistoryRepository
                .Received()
                .Update(Arg.Is<ProviderFeedbackRequestHistory>(
                    p => p.Id == 1
                         //&& p.ProviderCount == 1
                         && p.ModifiedBy == "TestUser"
                ));
        }
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Sets_Expected_Values_In_First_Call()
        {
            var history = _receivedProviderFeedbackRequestHistories.First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(ProviderFeedbackRequestStatus.Processing.ToString());
            history.StatusMessage.Should().BeNullOrEmpty();
            history.ProviderCount.Should().Be(1);
            history.ModifiedBy.Should().Be("TestUser");
        }
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Sets_Expected_Values_In_Second_Call()
        {
            var history = _receivedProviderFeedbackRequestHistories.Skip(1).First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(ProviderFeedbackRequestStatus.Error.ToString());
            history.StatusMessage.Should().NotBeNullOrEmpty();
            history.ProviderCount.Should().Be(0);
            history.ModifiedBy.Should().Be("TestUser");
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
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
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Not_Called()
        {
            _emailHistoryService
                .DidNotReceiveWithAnyArgs()
                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
