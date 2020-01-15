﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class ProviderFeedbackFixture
    {
        internal readonly IRepository<BankHoliday> BankHolidayRepository;
        internal readonly MatchingConfiguration Configuration;
        internal readonly ILogger<ProviderFeedbackService> Logger;

        public ProviderFeedbackFixture()
        {
            Configuration = new MatchingConfiguration
            {
                ProviderFeedbackEmailsEnabled = true,
                ProviderFeedbackWorkingDayInMonth = 10,
                SendEmailEnabled = true
            };

            Logger = Substitute.For<ILogger<ProviderFeedbackService>>();

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

            BankHolidayRepository =
                new GenericRepository<BankHoliday>(NullLogger<GenericRepository<BankHoliday>>.Instance, mockContext);

            BankHolidayRepository = Substitute.For<IRepository<BankHoliday>>();
            BankHolidayRepository
                .GetManyAsync(Arg.Any<Expression<Func<BankHoliday, bool>>>())
                .Returns(new List<BankHoliday>().AsQueryable());
        }

        public bool DoTokensContainExpectedValues(IDictionary<string, string> tokens, IDictionary<string, string> expectedResults)
        {
            if (tokens == null)
            {
                return false;
            }

            foreach (var expectedResult in expectedResults)
            {
                if (!(tokens.ContainsKey(expectedResult.Key) &&
                    tokens[expectedResult.Key] == expectedResult.Value))
                {
                    Console.WriteLine(expectedResult.Value);
                    Console.WriteLine(tokens[expectedResult.Key]);
                    System.Diagnostics.Debug.WriteLine($"Debug: {expectedResult.Value}");
                    System.Diagnostics.Debug.WriteLine($"Debug: {tokens[expectedResult.Key]}");

                    return false;
                }
            }

            return true;
        }
    }
}
