using System;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    [Trait("DateTime", "Data Tests")]
    public class DateTimeExtensionsTests
    {
        [Theory(DisplayName = "GetTimeWithDate Data Tests")]
        [InlineData("01/01/2000 16:50:16", "on", "04:50pm on 01 January 2000")]
        [InlineData("01/04/2017 22:22:16", "on", "11:22pm on 01 April 2017")]
        [InlineData("18/08/2018 06:22:16", "on", "07:22am on 18 August 2018")]
        [InlineData("31/12/2019 13:02:16", "on", "01:02pm on 31 December 2019")]
        public void GetTimeWithDateDataTests(string dateTime, string seperator, string result)
        {
            var dt = DateTime.Parse(dateTime);
            dt.GetTimeWithDate(seperator).Should().Be(result);
        }
    }
}