using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Providers.DateTimeProvider
{
    public class When_DateTimeProvider_GetNthWorkingDayDate_Is_Called
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new DateTime(2019, 11, 1), 1, new List<DateTime> { new(2019, 12, 25), new(2019, 12, 26) }, new DateTime(2019, 11, 1) },
                new object[] { new DateTime(2019, 12, 26), 1, new List<DateTime> { new(2019, 12, 25), new(2019, 12, 26) }, new DateTime(2019, 12, 2) },
                new object[] { new DateTime(2019, 12, 26), 10, new List<DateTime> { new(2019, 12, 25), new(2019, 12, 26) }, new DateTime(2019, 12, 13) },
                new object[] { new DateTime(2019, 5, 5), 10, new List<DateTime> { new(2019, 5, 6), new(2019, 12, 26) }, new DateTime(2019, 5, 15) }
            };

        [Theory(DisplayName = "DateTimeProvider.GetNthWorkingDayDate Data Tests")]
        [MemberData(nameof(Data))]
        public void Then_Expected_Results_Are_Returned(DateTime dateTime, int n, IList<DateTime> holidays, DateTime expected)
        {
            var dateTimeProvider = new Application.Services.DateTimeProvider();
            var result = dateTimeProvider.GetNthWorkingDayDate(dateTime, n, holidays);
            result.Should().Be(expected);
        }
    }
}
