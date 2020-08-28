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
    public class When_ReferralEmailService_Is_Called_To_Send_Employer_Email_With_No_Opportunity_Item_Ids
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public When_ReferralEmailService_Is_Called_To_Send_Employer_Email_With_No_Opportunity_Item_Ids()
        {
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            _backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();
            
            var mapper = Substitute.For<IMapper>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _emailService = Substitute.For<IEmailService>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            
            _backgroundProcessHistoryRepository.GetSingleOrDefaultAsync(
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

            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var referralEmailService = new ReferralEmailService(mapper, datetimeProvider, _emailService,
                _opportunityRepository, opportunityItemRepository, _backgroundProcessHistoryRepository, functionLogRepository);

            referralEmailService.SendEmployerReferralEmailAsync(1, new List<int>(), 1, "system").GetAwaiter().GetResult();
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
        public void Then_EmailService_SendEmail_Is_Not_Called()
        {
            _emailService
                .DidNotReceive()
                .SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<int?>(), Arg.Any<int?>(), 
                    Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_UpdateWithSpecifiedColumnsOnly_Is_Called_Exactly_Once()
        {
             _backgroundProcessHistoryRepository
                .Received(1)
                .UpdateWithSpecifiedColumnsOnlyAsync(Arg.Any<BackgroundProcessHistory>(),
                    Arg.Any<Expression<Func<BackgroundProcessHistory, object>>[]>());
        }
    }
}
