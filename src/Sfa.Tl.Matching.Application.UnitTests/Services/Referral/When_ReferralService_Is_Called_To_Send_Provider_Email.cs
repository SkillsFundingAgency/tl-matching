using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Provider_Email
    {
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_ReferralService_Is_Called_To_Send_Provider_Email()
        {
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            var backgroundProcessHistoryRepo = Substitute.For<IRepository<BackgroundProcessHistory>>();
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                NotificationsSystemId = "TLevelsIndustryPlacement"
            };

            var mapper = Substitute.For<IMapper>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            
            backgroundProcessHistoryRepo.GetSingleOrDefault(
                Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>()).Returns(new BackgroundProcessHistory
            {
                Id = 1,
                ProcessType = BackgroundProcessType.ProviderReferralEmail.ToString(),
                Status = BackgroundProcessHistoryStatus.Pending.ToString()
            });

            _opportunityRepository
                .GetProviderOpportunities(
                    Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new ValidOpportunityReferralDtoListBuilder().Build());

            var itemIds = new List<int>
            {
                1
            };

            var referralEmailService = new ReferralEmailService(mapper, configuration, datetimeProvider, _emailService,
                _emailHistoryService, _opportunityRepository, opportunityItemRepository, backgroundProcessHistoryRepo);

            referralEmailService.SendProviderReferralEmailAsync(1, itemIds, 1, "system").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetProviderOpportunities_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetProviderOpportunities(Arg.Any<int>(), Arg.Any<IEnumerable<int>>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_TemplateName()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "ProviderReferralV3"),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Primary_ToAddress()
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
        public void Then_EmailService_SendEmail_Is_Called_With_Secondary_ToAddress()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Is<string>(
                        toAddress => toAddress == "secondary.contact@provider.co.uk"),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Subject()
        {
            _emailService
                .Received(2)
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
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(
                        replyToAddress => replyToAddress == ""));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_ContactName_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("contact_name")),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_ProviderName_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("provider_name")
                        && tokens["provider_name"] == "Provider display name"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Route_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("route")
                                  && tokens["route"] == "agriculture, environmental and animal care"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Venue_Text_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("venue_text")
                                  && tokens["venue_text"] == "at Venue name (part of Provider display name) in Venuetown AA2 2AA"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Search_Radius_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("search_radius")
                                  && tokens["search_radius"] == "3.5"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Job_Role_List_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("job_role_list")
                                  && tokens["job_role_list"] == "* who is looking for this job role: Testing Job Title"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Business_Name_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_business_name")
                                  && tokens["employer_business_name"] == "Company"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Name_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_name")
                                  && tokens["employer_contact_name"] == "Employer Contact"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Contact_Number_Token()
        {
            _emailService
                .Received(2)
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
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_email")
                                  && tokens["employer_contact_email"] == "employer.contact@employer.co.uk"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Employer_Town_Postcode_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_town_postcode")
                                  && tokens["employer_town_postcode"] == "Town AA1 1AA"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Number_Of_Placements_Token()
        {
            _emailService
                .Received(2)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("number_of_placements")
                                  && tokens["number_of_placements"] == "at least 1"),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Called_Exactly_Once()
        {
            _emailHistoryService
                .Received(2)
                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}