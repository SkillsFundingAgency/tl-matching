using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Employer_Email
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_ReferralService_Is_Called_To_Send_Employer_Email()
        {
            _emailService = Substitute.For<IEmailService>();

            var logger = Substitute.For<ILogger<ReferralService>>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            _emailTemplateRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>())
                .Returns(new ValidEmailTemplateBuilder().Build());

            _opportunityRepository
                .GetEmployerReferrals(
                    Arg.Any<int>())
                .Returns(new ValidEmployerReferralDtoBuilder().Build());

            var referralService = new ReferralService(_emailService,
                _emailHistoryRepository, _emailTemplateRepository, _opportunityRepository,
                mapper, logger);

            referralService.SendEmployerReferralEmail(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetEmployerReferrals_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetEmployerReferrals(Arg.Any<int>());
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
                        templateName => templateName == "EmployerReferral"),
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
                        toAddress => toAddress == "employer.contact@employer.co.uk"),
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
                        subject => subject == "Industry Placement Matching Referral"),
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
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Name_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_name")
                                  && tokens["employer_contact_name"] == "Employer Contact"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Business_Name_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_business_name")
                        && tokens["employer_business_name"] == "Employer"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Number_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_number")
                                  && tokens["employer_contact_number"] == "020 123 4567"),
                    Arg.Any<string>());
        }
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Email_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_email")
                                  && tokens["employer_contact_email"] == "employer.contact@employer.co.uk"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Postcode_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_postcode")
                                  && tokens["employer_postcode"] == "AA1 1AA"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Route_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("route")
                                  && tokens["route"] == "agriculture, environmental and animal care"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Venue_Postcode_Token()
        {
            const string expectedProvidersList = "# Provider\r\n"
                                                 + "AA2 2AA\r\n"
                                                 + "Contact name: Provider Contact\r\n"
                                                 + "Telephone: 01777757777\r\n"
                                                 + "Email: primary.contact@provider.co.uk\r\n"
                                                 + "\r\n"
                                                 + "Has students learning: \r\n"
                                                 + "* Qualification 1\r\n"
                                                 + "* Qualification 2\r\n"
                                                 + "\r\n";
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("providers_list")
                                  && tokens["providers_list"] == expectedProvidersList),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Job_Role_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("job_role")
                                  && tokens["job_role"] == "Testing Job Title"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Number_Of_Placements_Token()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("number_of_placements")
                                  && tokens["number_of_placements"] == "at least one"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Any<EmailHistory>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_OpportunityId()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<EmailHistory>(email =>
                    email.OpportunityId == 1));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_EmailTemplateId()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<EmailHistory>(email =>
                    email.EmailTemplateId == 1));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_SentTo()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<EmailHistory>(email =>
                    email.SentTo == "employer.contact@employer.co.uk"));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_CreatedBy()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<EmailHistory>(email =>
                    email.CreatedBy == "CreatedBy"));
        }
    }
}
