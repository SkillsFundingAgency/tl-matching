using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Providers.DateTimeProvider
{
    public class When_DateTimeProvider_AddWorkingDays_Is_Called
    {
        [Fact]
        public void Then_Expected_Results_Are_Returned_For_One_Day_Backwards_With_No_Bank_Holidays()
        {
            var dateTime = new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc);
            var timeSpan = TimeSpan.Parse("-1.00:00:00");
            var holidays = new List<DateTime>();

            var expected = new DateTime(2019, 8, 19, 23, 59, 59, DateTimeKind.Utc);

            var dateTimeProvider = new Application.Services.DateTimeProvider();

            dateTimeProvider
                .AddWorkingDays(dateTime, timeSpan, holidays)
                .Should().Be(expected);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned_For_Zero_Days_With_No_Bank_Holidays()
        {
            var dateTime = new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc);
            var timeSpan = TimeSpan.Parse("00.00:00:00");
            var holidays = new List<DateTime>();

            var expected = new DateTime(2019, 8, 20, 23, 59, 59, DateTimeKind.Utc);

            var dateTimeProvider = new Application.Services.DateTimeProvider();

            dateTimeProvider
                .AddWorkingDays(dateTime, timeSpan, holidays)
                .Should().Be(expected);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned_For_One_Day_Forwards_With_No_Bank_Holidays()
        {
            var dateTime = new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc);
            var timeSpan = TimeSpan.Parse("01.00:00:00");
            var holidays = new List<DateTime>();

            var expected = new DateTime(2019, 8, 21, 23, 59, 59, DateTimeKind.Utc);

            var dateTimeProvider = new Application.Services.DateTimeProvider();

            dateTimeProvider
                .AddWorkingDays(dateTime, timeSpan, holidays)
                .Should().Be(expected);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned_For_Ten_Days_Backwards_Not_Spanning_Bank_Holiday()
        {
            var dateTime = new DateTime(2019, 8, 20, 16, 50, 55, DateTimeKind.Utc);
            var timeSpan = TimeSpan.Parse("-10.00:00:00");
            var holidays = new List<DateTime>();

            var expected = new DateTime(2019, 8, 6, 23, 59, 59, DateTimeKind.Utc);

            var dateTimeProvider = new Application.Services.DateTimeProvider();

            dateTimeProvider
                .AddWorkingDays(dateTime, timeSpan, holidays)
                .Should().Be(expected);
        }

        //[Fact]
        //public void Then_Expected_Results_Are_Returned_For_Ten_Days_Backwards_Spanning_Bank_Holiday()
        //{
        //    //new object[] { new DateTime(2019, 8, 27, 16, 50, 55, DateTimeKind.Utc),
        //    //"-10.00:00:00", new List<DateTime>(), new DateTime(2019, 8, 13, 23, 59, 59, DateTimeKind.Utc) },

        //}

        [Fact]
        public void Then_Expected_Results_Are_Returned_For_Ten_Days_Backwards_Spanning_Bank_Holiday()
        {
            var dateTime = new DateTime(2019, 8, 27, 16, 50, 55, DateTimeKind.Utc);
            var timeSpan = TimeSpan.Parse("-10.00:00:00");
            var holidays = new List<DateTime>
            {
                new DateTime(2019, 8, 26)
            };

            var expected = new DateTime(2019, 8, 12, 23, 59, 59, DateTimeKind.Utc);

            var dateTimeProvider = new Application.Services.DateTimeProvider();

            dateTimeProvider
                .AddWorkingDays(dateTime, timeSpan, holidays)
                .Should().Be(expected);
        }

    }
}
