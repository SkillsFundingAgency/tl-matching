//using System.Collections.Generic;
//using NSubstitute;
//using Sfa.Tl.Matching.Application.Interfaces;
//using Sfa.Tl.Matching.Application.Services;
//using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
//using Sfa.Tl.Matching.Data.Interfaces;
//using Sfa.Tl.Matching.Models.Configuration;
//using Xunit;

//namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
//{
//    public class When_ReferralService_Is_Called_To_Send_Employer_Email
//    {
//        private readonly IEmailService _emailService;
//        private readonly IEmailHistoryService _emailHistoryService;
//        private readonly IOpportunityRepository _opportunityRepository;

//        public When_ReferralService_Is_Called_To_Send_Employer_Email()
//        {
//            var configuration = new MatchingConfiguration
//            {
//                SendEmailEnabled = true,
//                NotificationsSystemId = "TLevelsIndustryPlacement"
//            };

//            _emailService = Substitute.For<IEmailService>();
//            _emailHistoryService = Substitute.For<IEmailHistoryService>();

//            _opportunityRepository = Substitute.For<IOpportunityRepository>();

//            _opportunityRepository
//                .GetEmployerReferrals(
//                    Arg.Any<int>())
//                .Returns(new ValidEmployerReferralDtoBuilder().Build());

//            var referralService = new ReferralService(
//                configuration, 
//                _emailService, _emailHistoryService,
//                _opportunityRepository);

//            referralService.SendEmployerReferralEmail(1).GetAwaiter().GetResult();
//        }

//        [Fact]
//        public void Then_OpportunityRepository_GetEmployerReferrals_Is_Called_Exactly_Once()
//        {
//            _opportunityRepository
//                .Received(1)
//                .GetEmployerReferrals(Arg.Any<int>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_TemplateName()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Is<string>(
//                        templateName => templateName == "EmployerReferralComplex"),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<IDictionary<string, string>>(),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_ToAddress()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Is<string>(
//                        toAddress => toAddress == "employer.contact@employer.co.uk"),
//                    Arg.Any<string>(),
//                    Arg.Any<IDictionary<string, string>>(),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Subject()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<string>(
//                        subject => subject == "Your industry placement referral – ESFA National Apprenticeship Service"),
//                    Arg.Any<IDictionary<string, string>>(),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_ReplyToAddress()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<IDictionary<string, string>>(),
//                    Arg.Is<string>(
//                        replyToAddress => replyToAddress == ""));
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Name_Token()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("employer_contact_name")
//                                  && tokens["employer_contact_name"] == "Employer Contact"),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Business_Name_Token()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("employer_business_name")
//                        && tokens["employer_business_name"] == "Employer"),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Number_Token()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("employer_contact_number")
//                                  && tokens["employer_contact_number"] == "020 123 4567"),
//                    Arg.Any<string>());
//        }
//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Email_Token()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("employer_contact_email")
//                                  && tokens["employer_contact_email"] == "employer.contact@employer.co.uk"),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Postcode_Token()
//        {
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("employer_postcode")
//                                  && tokens["employer_postcode"] == "AA1 1AA"),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailService_SendEmail_Is_Called_With_Venue_Postcode_Token()
//        {
//            const string expectedProvidersList = "# Town AA1 1AA\r\n"
//                                                 + "*Job role: Job Role\r\n"
//                                                 + "*Students wanted: at least 1\r\n"
//                                                 + "*Providers selected: Provider name\r\n"
//                                                 + "\r\n";
//            _emailService
//                .Received(1)
//                .SendEmail(Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Any<string>(),
//                    Arg.Is<IDictionary<string, string>>(
//                        tokens => tokens.ContainsKey("placements_list")
//                                  && tokens["placements_list"] == expectedProvidersList),
//                    Arg.Any<string>());
//        }

//        [Fact]
//        public void Then_EmailHistoryService_SaveEmailHistory_Is_Called_Exactly_Once()
//        {
//            _emailHistoryService
//                .Received(1)
//                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
//        }
//    }
//}
