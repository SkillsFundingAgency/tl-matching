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
    public class When_ReferralService_Is_Called_To_Send_Employer_Email
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_ReferralService_Is_Called_To_Send_Employer_Email()
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
                    ProcessType = BackgroundProcessType.EmployerReferralEmail.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString()
                });

            _opportunityRepository
                .GetEmployerReferralsAsync(
                    Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new ValidEmployerReferralDtoBuilder()
                    .AddSecondaryContact()
                    .Build());

            var itemIds = new List<int>
            {
                1
            };

            var referralEmailService = new ReferralEmailService(mapper, datetimeProvider, _emailService,
                _opportunityRepository, opportunityItemRepository, backgroundProcessHistoryRepo);

            referralEmailService.SendEmployerReferralEmailAsync(1, itemIds, 1, "system").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetEmployerReferrals_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetEmployerReferralsAsync(
                    Arg.Any<int>(), Arg.Any<IEnumerable<int>>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Is<string>(
                        templateName => templateName == "EmployerReferralV4"),
                    Arg.Is<string>(
                        toAddress => toAddress == "employer.contact@employer.co.uk"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("employer_contact_name")
                                  && tokens["employer_contact_name"] == "Employer Contact"
                                  && tokens.ContainsKey("employer_business_name")
                                  && tokens["employer_business_name"] == "Employer"
                                  && tokens.ContainsKey("employer_contact_number")
                                  && tokens["employer_contact_number"] == "020 123 4567"
                                  && tokens.ContainsKey("employer_contact_email")
                                  && tokens["employer_contact_email"] == "employer.contact@employer.co.uk"), 
                    Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Placements_List()
        {
            const string expectedPlacementsList = "# WorkplaceTown WorkplacePostcode\r\n"
                                                 + "* Job role: Job Role\r\n"
                                                 + "* Students wanted: 2\r\n"
                                                 + "* First provider selected: Venue Name part of Display Name (ProviderPostcode)\r\n"
                                                 + "Primary contact: Primary Contact (Telephone: 020 123 3210; Email: primary.contact@provider.ac.uk)\r\n"
                                                 + "Secondary contact: Secondary Contact (Telephone: 021 456 0987; Email: secondary.contact@provider.ac.uk)\r\n"
                                                 + "\r\n";

            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("placements_list")
                                  && tokens["placements_list"] == expectedPlacementsList), Arg.Any<string>());
        }
    }
}
