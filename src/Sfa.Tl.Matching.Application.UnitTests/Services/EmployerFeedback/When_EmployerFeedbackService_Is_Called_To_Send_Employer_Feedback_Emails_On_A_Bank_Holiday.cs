using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails_On_A_Bank_Holiday
        : IClassFixture<EmployerFeedbackFixture>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly int _result;

        public When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails_On_A_Bank_Holiday(
            EmployerFeedbackFixture testFixture)
        {
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 8, 26));
            _dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-8-15 23:59:59"));
            _dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(true);

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<UsernameForFeedbackSentDto, OpportunityItem>(
                            _dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new EmployerFeedbackDtoListBuilder().Build());

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            var bankHolidays = new BankHolidayListBuilder().Build().AsQueryable();

            var mockSet = Substitute.For<DbSet<BankHoliday>, IAsyncEnumerable<BankHoliday>, IQueryable<BankHoliday>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<BankHoliday>)mockSet).GetEnumerator()
                .Returns(new FakeAsyncEnumerator<BankHoliday>(bankHolidays.GetEnumerator()));
            ((IQueryable<BankHoliday>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<BankHoliday>(bankHolidays.Provider));
            ((IQueryable<BankHoliday>)mockSet).Expression.Returns(bankHolidays.Expression);
            ((IQueryable<BankHoliday>)mockSet).ElementType.Returns(bankHolidays.ElementType);
            ((IQueryable<BankHoliday>)mockSet).GetEnumerator().Returns(bankHolidays.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions);
            mockContext.Set<BankHoliday>().Returns(mockSet);

            IRepository<BankHoliday> bankHolidayRepository =
                new GenericRepository<BankHoliday>(NullLogger<GenericRepository<BankHoliday>>.Instance, mockContext);

            var employerFeedbackService = new EmployerFeedbackService(
                mapper, testFixture.Configuration, testFixture.Logger,
                _dateTimeProvider, 
                _emailService, _emailHistoryService, bankHolidayRepository, 
                _opportunityRepository);

            _result = employerFeedbackService
                .SendEmployerFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_DateTimeProvider_IsHoliday_Is_Called_Exactly_Once()
        {
            _dateTimeProvider
                .Received(1)
                .GetReferralDateAsync(Arg.Any<IList<DateTime>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_DateTimeProvider_AddWorkingDays_Is_Not_Called()
        {
            _dateTimeProvider
                .DidNotReceive()
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>());
        }

        [Fact]
        public void Then_OpportunityRepository_GetReferralsForEmployerFeedbackAsync_Is_Not_Called()
        {
            _opportunityRepository
                .DidNotReceive()
                .GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>());
        }

        [Fact]public void Then_OpportunityItemRepository_UpdateManyWithSpecifedColumnsOnly_Is_Not_Called()
        {
            _opportunityItemRepository
                .DidNotReceive()
                .UpdateManyWithSpecifedColumnsOnlyAsync(Arg.Any<IList<OpportunityItem>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>()
                );
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Not_Called_Exactly_Once()
        {
            _emailService
                .DidNotReceive()
                .SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>());
        }
        
        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Not_Called()
        {
            _emailHistoryService
                .DidNotReceive()
                .SaveEmailHistoryAsync(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(),
                    Arg.Any<string>(), Arg.Any<string>());
        }
       
        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(0);
        }
    }
}
