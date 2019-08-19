using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Providers.DateTimeProvider
{
    public class When_DateTimeProvider_AddWorkingDays_Is_Called
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), -1, new List<DateTime>(), new DateTime(2019, 8, 19, 16, 50, 55, DateTimeKind.Utc) },
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), 0, new List<DateTime>(), new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc) },
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), 1, new List<DateTime>(), new DateTime(2019, 8, 21, 16, 50, 55, DateTimeKind.Utc) },
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), -10, new List<DateTime>(), new DateTime(2019, 8, 6, 16, 50, 55, DateTimeKind.Utc) },
                new object[] { new DateTime(2019, 8, 27, 16, 50, 55, DateTimeKind.Utc), -10, new List<DateTime>(), new DateTime(2019, 8, 13, 16, 50, 55, DateTimeKind.Utc) },
                new object[] { new DateTime(2019, 8, 27, 16, 50, 55, DateTimeKind.Utc), -10, new List<DateTime> { new DateTime(2019, 8, 26)}, new DateTime(2019, 8, 12, 16, 50, 55, DateTimeKind.Utc) }
            };

        [Theory(DisplayName = "DateTimeProvider.AddWorkingDays Data Tests")]
        [MemberData(nameof(Data))]
        public void Then_Expected_Results_Are_Returned(DateTime dateTime, int days, IList<DateTime> holidays, DateTime result)
        {
            var dateTimeProvider = new Application.Services.DateTimeProvider();
            dateTimeProvider.AddWorkingDays(dateTime, days, holidays)
                .Should().Be(result);
        }
    }
}
