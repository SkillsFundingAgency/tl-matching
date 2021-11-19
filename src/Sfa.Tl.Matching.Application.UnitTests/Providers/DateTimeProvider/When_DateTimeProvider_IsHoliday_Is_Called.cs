using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Providers.DateTimeProvider
{
    public class When_DateTimeProvider_IsHoliday_Is_Called
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), null, false },
                new object[] { new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc), new List<DateTime>(), false },
                new object[] { new DateTime(2019, 8, 26, 16, 50, 55, DateTimeKind.Utc), new List<DateTime> { new(2019, 8, 26), new(2019, 12, 25)}, true },
                new object[] { new DateTime(2019, 8, 27, 16, 50, 55, DateTimeKind.Utc), new List<DateTime> { new(2019, 8, 26), new(2019, 12, 25)}, false }
            };

        [Theory(DisplayName = "DateTimeProvider.IsHoliday Data Tests")]
        [MemberData(nameof(Data))]
        public void Then_Expected_Results_Are_Returned(DateTime dateTime, IList<DateTime> holidays, bool result)
        {
            var dateTimeProvider = new Application.Services.DateTimeProvider();
            dateTimeProvider.IsHoliday(dateTime, holidays)
                .Should().Be(result);
        }
    }
}
