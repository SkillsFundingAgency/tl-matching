using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails
    {
        private readonly IEmailService _emailService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;

        public When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);
            _emailService = Substitute.For<IEmailService>();
            var logger = Substitute.For<ILogger<ProviderFeedbackService>>();
            var messageQueueService = Substitute.For<IMessageQueueService>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            _providerFeedbackRequestHistoryRepository = Substitute.For<IRepository<ProviderFeedbackRequestHistory>>();
            _providerFeedbackRequestHistoryRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderFeedbackRequestHistory, bool>>>())
                .Returns(new ProviderFeedbackRequestHistoryBuilder().Build());

            var providerFeedbackService = new ProviderFeedbackService(_emailService,
                _providerRepository, _providerFeedbackRequestHistoryRepository,
                messageQueueService, dateTimeProvider, mapper, logger);

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
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Is_Called_Exactly_Once()
        {
            _providerFeedbackRequestHistoryRepository
                .Received(1)
                .Update(Arg.Is<ProviderFeedbackRequestHistory>(
                    p => p.Id == 1
                         && p.Status == (short)ProviderFeedbackRequestStatus.Sent
                         && p.ProviderCount == 1
                         && p.ModifiedBy == "TestUser"
                         ));
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_TemplateName()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "ProviderQuarterlyUpdate"),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_ToAddress()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Is<string>(
                        toAddress => toAddress == "primary.contact@provider.co.uk"),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Subject()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<string>(
                        subject => subject == "Industry Placement Matching Provider Update"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_ReplyToAddress()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(
                        replyToAddress => replyToAddress == ""));
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_ProviderName_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("provider_name")
                        && tokens["provider_name"] == "Provider Name"),
                    Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Primary_Contact_Name_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("primary_contact_name")
                                  && tokens["primary_contact_name"] == "Provider Contact"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Primary_Contact_Email_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("primary_contact_email")
                                  && tokens["primary_contact_email"] == "primary.contact@provider.co.uk"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Primary_Contact_Phone_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("primary_contact_phone")
                                  && tokens["primary_contact_phone"] == "01777757777"),
                    Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Secondary_Contact_Details_Token()
        {
            const string expectedSecondaryDetailsList = "We also have the following secondary contact for Provider Name:\r\n"
                                                      + "* Name: SecondaryContact\r\n"
                                                      + "* Email: secondary@contact.co.uk\r\n"
                                                      + "* Telephone: 01234559999\r\n";

            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("secondary_contact_details")
                                  && tokens["secondary_contact_details"] == expectedSecondaryDetailsList),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Venues_and_Qualifications_List_Token()
        {
            const string expectedProviderVenueQualificationsList = "AA1 1AA:\r\n"
                                                 + "* 10042982: Qualification 1\r\n"
                                                 + "* 60165522: Qualification 2\r\n"
                                                 + "\r\n";

            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("venues_and_qualifications_list")
                                  && tokens["venues_and_qualifications_list"] == expectedProviderVenueQualificationsList),
                    Arg.Any<string>());
        }
    }
}
