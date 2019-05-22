using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly ProviderFeedbackFixture _testFixture;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;
        private readonly IList<ProviderFeedbackRequestHistory> _receivedProviderFeedbackRequestHistories;

        public When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails(ProviderFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var messageQueueService = Substitute.For<IMessageQueueService>();
            
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
                        ModifiedOn = x.ModifiedOn,
                        ModifiedBy = x.ModifiedBy
                    }
                )));

            var providerFeedbackService = new ProviderFeedbackService(
                _testFixture.Configuration, _testFixture.Logger,
                    _emailService, _emailHistoryService,
                    _providerRepository, _providerFeedbackRequestHistoryRepository,
                    messageQueueService, _testFixture.DateTimeProvider);

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
            //Can't check Status here because NSubstitute only remembers the last one
            _providerFeedbackRequestHistoryRepository
                .Received()
                .Update(Arg.Is<ProviderFeedbackRequestHistory>(
                    p => p.Id == 1
                         && p.ProviderCount == 1
                         && p.ModifiedBy == "TestUser"
                ));
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Sets_Expected_Values_In_First_Call()
        {
            var history = _receivedProviderFeedbackRequestHistories.First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(ProviderFeedbackRequestStatus.Processing.ToString());
            history.ProviderCount.Should().Be(1);
            history.ModifiedBy.Should().Be("TestUser");
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Sets_Expected_Values_In_Second_Call()
        {
            var history = _receivedProviderFeedbackRequestHistories.Skip(1).First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(ProviderFeedbackRequestStatus.Complete.ToString());
            history.ProviderCount.Should().Be(1);
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
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "ProviderQuarterlyUpdate"),
                    Arg.Is<string>(toAddress => toAddress == "primary.contact@provider.co.uk"),
                    Arg.Is<string>(subject => subject == "Industry Placement Matching Provider Update"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(replyToAddress => replyToAddress == ""));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            const string expectedSecondaryDetailsList = "We also have the following secondary contact for Provider Name:\r\n"
                                                        + "* Name: SecondaryContact\r\n"
                                                        + "* Email: secondary@contact.co.uk\r\n"
                                                        + "* Telephone: 01234559999\r\n";
            const string expectedProviderVenueQualificationsList = "AA1 1AA:\r\n"
                                                                   + "* 10042982: Qualification 1\r\n"
                                                                   + "* 60165522: Qualification 2\r\n"
                                                                   + "\r\n";
            
            var expectedResults = new Dictionary<string, string>
            {
                { "provider_name",  "Provider Name" },
                { "primary_contact_name", "Provider Contact" },
                { "primary_contact_email", "primary.contact@provider.co.uk" },
                { "primary_contact_phone", "01777757777" },
                { "secondary_contact_details", expectedSecondaryDetailsList },
                { "provider_has_venues", "yes" },
                { "provider_has_no_venues", "no" },
                { "venues_and_qualifications_list", expectedProviderVenueQualificationsList },
            };

            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedResults)),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Called_Exactly_Once()
        {
            _emailHistoryService
                .Received(1)
                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
