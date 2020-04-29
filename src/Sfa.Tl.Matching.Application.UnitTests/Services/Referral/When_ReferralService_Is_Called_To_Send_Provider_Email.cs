using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Provider_Email
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;

        private readonly IDictionary<string, string> _contactNames = new Dictionary<string, string>();

        public When_ReferralService_Is_Called_To_Send_Provider_Email()
        {
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            var backgroundProcessHistoryRepo = Substitute.For<IRepository<BackgroundProcessHistory>>();

            var mapper = Substitute.For<IMapper>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _emailService = Substitute.For<IEmailService>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            backgroundProcessHistoryRepo.GetSingleOrDefaultAsync(
                Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>()).Returns(new BackgroundProcessHistory
                {
                    Id = 1,
                    ProcessType = BackgroundProcessType.ProviderReferralEmail.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString()
                });

            _emailService
                .When(x => x.SendEmailAsync(Arg.Any<int?>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>()))
                .Do(x =>
                {
                    var address = x.ArgAt<string>(2);
                    var tokens = x.Arg<Dictionary<string, string>>();
                    if (tokens.TryGetValue("contact_name", out var contact))
                    {
                        _contactNames[address] = contact;
                    }
                });

            _opportunityRepository
                .GetProviderOpportunitiesAsync(
                    Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new ValidOpportunityReferralDtoListBuilder().Build());

            var itemIds = new List<int>
            {
                1
            };

            var referralEmailService = new ReferralEmailService(mapper, datetimeProvider, _emailService,
                _opportunityRepository, opportunityItemRepository, backgroundProcessHistoryRepo);

            referralEmailService.SendProviderReferralEmailAsync(1, itemIds, 1, "system").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetProviderOpportunities_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetProviderOpportunitiesAsync(Arg.Any<int>(), Arg.Any<IEnumerable<int>>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Twice_With_Expected_Parameters()
        {
            _emailService
                .Received(2)
                .SendEmailAsync(Arg.Any<int?>(),
                    Arg.Is<string>(
                        templateName => templateName == "ProviderReferralV5"),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Twice_With_CreatedBy()
        {
            _emailService
                .Received(2)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(
                        createdBy => createdBy == "CreatedBy"));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Once_With_Primary_ToAddress()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(),
                    Arg.Any<string>(),
                    Arg.Is<string>(
                        toAddress => toAddress == "primary.contact@provider.co.uk"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Once_With_Secondary_ToAddress()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(),
                    Arg.Any<string>(),
                    Arg.Is<string>(
                        toAddress => toAddress == "secondary.contact@provider.co.uk"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Contact_Name_Tokens()
        {
            _contactNames.Should().ContainKey("primary.contact@provider.co.uk");
            _contactNames["primary.contact@provider.co.uk"].Should().Be("Provider Contact");
         
            _contactNames.Should().ContainKey("secondary.contact@provider.co.uk");
            _contactNames["secondary.contact@provider.co.uk"].Should().Be("Provider Secondary Contact");
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            _emailService
                .Received(2)
                .SendEmailAsync(Arg.Any<int?>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("provider_name")
                                  && tokens["provider_name"] == "Provider display name"
                                  && tokens.ContainsKey("route")
                                  && tokens["route"] == "agriculture, environmental and animal care"
                                  && tokens.ContainsKey("venue_text")
                                  && tokens["venue_text"] == "at Venue name in Venuetown AA2 2AA"
                                  && tokens.ContainsKey("search_radius")
                                  && tokens["search_radius"] == "3.5"
                                  && tokens.ContainsKey("job_role_list")
                                  && tokens["job_role_list"] == "* looking for this job role: Testing Job Title"
                                  && tokens.ContainsKey("employer_business_name")
                                  && tokens["employer_business_name"] == "Company"
                                  && tokens.ContainsKey("employer_contact_name")
                                  && tokens["employer_contact_name"] == "Employer Contact"
                                  && tokens.ContainsKey("employer_contact_number")
                                  && tokens["employer_contact_number"] == "020 123 4567"
                                  && tokens.ContainsKey("employer_contact_email")
                                  && tokens["employer_contact_email"] == "employer.contact@employer.co.uk"
                                  && tokens.ContainsKey("employer_town_postcode")
                                  && tokens["employer_town_postcode"] == "Town AA1 1AA"
                                  && tokens.ContainsKey("number_of_placements")
                                  && tokens["number_of_placements"] == "at least 1"),
                    Arg.Any<string>());
        }
    }
}