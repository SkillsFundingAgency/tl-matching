using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    public class When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails
        : IClassFixture<EmployerFeedbackFixture>
    {
        private readonly EmployerFeedbackFixture _testFixture;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly int _result;

        public When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails(
            EmployerFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 9, 1));
            dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-8-15 23:59:59"));
            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            _emailService = Substitute.For<IEmailService>();
            
            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<UsernameForFeedbackSentDto, Domain.Models.Opportunity>(
                            dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();

            _opportunityRepository.GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new EmployerFeedbackDtoListBuilder().Build());

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

            dateTimeProvider
                .GetReferralDateAsync(Arg.Any<IList<DateTime>>(), testFixture.Configuration.EmployerFeedbackTimeSpan)
                .Returns(DateTime.Parse("2019-8-15 23:59:59"));

            backgroundProcessHistoryRepository.CreateAsync(Arg.Any<BackgroundProcessHistory>()).Returns(Task.FromResult(1));

            backgroundProcessHistoryRepository
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>())
                .Returns(Task.FromResult(new BackgroundProcessHistory
                {
                    Id = 1,
                    ProcessType = "Test",
                    Status = "Pending"
                }));

            var employerFeedbackService = new EmployerFeedbackService(
                mapper, _testFixture.Configuration, _testFixture.Logger,
                dateTimeProvider,
                _emailService, bankHolidayRepository,
                _opportunityRepository, backgroundProcessHistoryRepository);

            _result = employerFeedbackService
                .SendFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetReferralsForEmployerFeedbackAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_OpportunityItemRepository_UpdateManyWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .UpdateManyWithSpecifedColumnsOnlyAsync(Arg.Any<IList<Domain.Models.Opportunity>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>()
                );
        }

        [Fact]
        public void Then_OpportunityItemRepository_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Two_Items_With_Expected_Values()
        {
            _opportunityRepository
                .Received(1)
                .UpdateManyWithSpecifedColumnsOnlyAsync(Arg.Is<IList<Domain.Models.Opportunity>>(
                        o => o.Count == 1
                             && o[0].Id == 1
                             && o[0].EmployerFeedbackSentOn == new DateTime(2019, 9, 1)
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 9, 1))
                    ),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>());
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Is<string>(
                        templateName => templateName == "EmployerFeedback"),
                    Arg.Is<string>(toAddress => toAddress == "primary.contact@employer.co.uk"),
                    Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            var expectedResults = new Dictionary<string, string>
            {
                {"employer_contact_name", "Employer Contact"},
            };

            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedResults)), Arg.Any<string>());
        }
        
        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(1);
        }
    }
}

