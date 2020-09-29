using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail
{
    public class When_ReferralEmailService_Is_Called_To_Send_Employer_Email_With_Provider_Secondary_Contact_With_No_Email
    {
        private readonly IEmailService _emailService;

        public When_ReferralEmailService_Is_Called_To_Send_Employer_Email_With_Provider_Secondary_Contact_With_No_Email()
        {
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            var backgroundProcessHistoryRepo = Substitute.For<IRepository<BackgroundProcessHistory>>();

            var mapper = Substitute.For<IMapper>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _emailService = Substitute.For<IEmailService>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            
            backgroundProcessHistoryRepo.GetSingleOrDefaultAsync(
                Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>()).Returns(new BackgroundProcessHistory
                {
                    Id = 1,
                    ProcessType = BackgroundProcessType.EmployerReferralEmail.ToString(),
                    Status = BackgroundProcessHistoryStatus.Pending.ToString()
                });

            opportunityRepository
                .GetEmployerReferralsAsync(
                    Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new ValidEmployerReferralDtoBuilder()
                    .AddSecondaryContact(includeEmail: false)
                    .Build());

            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var itemIds = new List<int>
            {
                1
            };

            var referralEmailService = new ReferralEmailService(mapper, datetimeProvider, _emailService,
                opportunityRepository, opportunityItemRepository, backgroundProcessHistoryRepo, functionLogRepository);

            referralEmailService.SendEmployerReferralEmailAsync(1, itemIds, 1, "system").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Placements_List()
        {
            const string expectedPlacementsList = "# WorkplaceTown WorkplacePostcode\r\n"
                                                 + "* Job role: Job Role\r\n"
                                                 + "* Students wanted: 2\r\n"
                                                 + "* First provider selected: Venue Name part of Display Name (ProviderPostcode)\r\n"
                                                 + "Primary contact: Primary Contact (Telephone: 020 123 3210; Email: primary.contact@provider.ac.uk)\r\n"
                                                 + "Secondary contact: Secondary Contact (Telephone: 021 456 0987)\r\n"
                                                 + "\r\n";

            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<int?>(), Arg.Any<int?>(), 
                    Arg.Is<IDictionary<string, string>>(
                    tokens => tokens.ContainsKey("placements_list")
                              && tokens["placements_list"] == expectedPlacementsList), Arg.Any<string>());
        }
    }
}
