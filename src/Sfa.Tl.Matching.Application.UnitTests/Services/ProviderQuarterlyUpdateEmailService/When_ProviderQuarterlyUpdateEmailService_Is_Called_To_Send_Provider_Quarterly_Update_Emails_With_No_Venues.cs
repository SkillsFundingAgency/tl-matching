using System;
using System.Collections.Generic;
using System.Linq;
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
    public class When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_With_No_Venues
        : IClassFixture<ProviderQuarterlyUpdateEmailFixture>
    {
        private readonly ProviderQuarterlyUpdateEmailFixture _testFixture;
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;
        private readonly IList<BackgroundProcessHistory> _receivedProviderQuarterlyUpdateEmailRequestHistories;
        private readonly int _result;

        public When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_With_No_Venues(ProviderQuarterlyUpdateEmailFixture testFixture)
        {
            _testFixture = testFixture;

            _emailService = Substitute.For<IEmailService>();
            
            var messageQueueService = Substitute.For<IMessageQueueService>();
           
            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().BuildWithNoVenues());

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
                        ModifiedOn = x.ModifiedOn,
                        ModifiedBy = x.ModifiedBy
                    }
                )));

            var providerQuarterlyUpdateEmailService = new Application.Services.ProviderQuarterlyUpdateEmailService(
                _testFixture.Logger,
                    _emailService,
                    _providerRepository, _backgroundProcessHistoryRepository,
                    messageQueueService, _testFixture.DateTimeProvider);

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
            //Can't check Status here because NSubstitute only remembers the last one
            _backgroundProcessHistoryRepository
                .Received()
                .UpdateAsync(Arg.Is<BackgroundProcessHistory>(
                    p => p.Id == 1
                         && p.RecordCount == 1
                         && p.ModifiedBy == "TestUser"
                ));
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Sets_Expected_Values_In_First_Call()
        {
            var history = _receivedProviderQuarterlyUpdateEmailRequestHistories.First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(BackgroundProcessHistoryStatus.Processing.ToString());
            history.RecordCount.Should().Be(1);
            history.ModifiedBy.Should().Be("TestUser");
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Sets_Expected_Values_In_Second_Call()
        {
            var history = _receivedProviderQuarterlyUpdateEmailRequestHistories.Skip(1).First();
            history.Id.Should().Be(1);
            history.Status.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());
            history.RecordCount.Should().Be(1);
            history.ModifiedBy.Should().Be("TestUser");
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Is<string>(templateName => templateName == "ProviderQuarterlyUpdate"), 
                    Arg.Is<string>(toAddress => toAddress == "primary.contact@provider.co.uk"),
                    Arg.Any<int?>(), Arg.Any<int?>(), 
                    Arg.Any<IDictionary<string, string>>(), 
                    Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            const string expectedSecondaryDetailsList = "We also have the following secondary contact for Provider Name:\r\n"
                                                        + "* Name: SecondaryContact\r\n"
                                                        + "* Email: secondary@contact.co.uk\r\n"
                                                        + "* Telephone: 01234559999\r\n";
            const string expectedProviderVenueQualificationsList = "";
            
            var expectedResults = new Dictionary<string, string>
            {
                { "provider_name",  "Provider Name" },
                { "primary_contact_name", "Provider Contact" },
                { "primary_contact_email", "primary.contact@provider.co.uk" },
                { "primary_contact_phone", "01777757777" },
                { "secondary_contact_details", expectedSecondaryDetailsList },
                { "provider_has_venues", "no" },
                { "provider_has_no_venues", "yes" },
                { "venues_and_qualifications_list", expectedProviderVenueQualificationsList }
            };

            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<int?>(), Arg.Any<int?>(),
                    Arg.Is<IDictionary<string, string>>(tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedResults)),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(1);
        }
    }
}
