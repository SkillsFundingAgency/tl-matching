using System;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    [Trait("TimeSpan", "Data Tests")]
    public class TimeSpanExtensionsTests
    {
        [Theory(DisplayName = "SecondsToReadableString Real World Examples Data Tests")]
        [InlineData(0, 0)]
        [InlineData(29, 0)]
        [InlineData(30, 60)]
        [InlineData(31, 60)]
        [InlineData(59, 60)]
        [InlineData(60, 60)]
        [InlineData(61, 60)]
        [InlineData(80, 60)]
        [InlineData(90, 120)]
        [InlineData(3600, 3600)]
        [InlineData(3629, 3600)]
        [InlineData(3631, 3660)]
        public void RoundToNearestMinuteDataTests(long seconds, int result)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            var rounded = ts.RoundToNearestMinute();
            rounded.TotalSeconds.Should().Be(result);
        }

        [Theory(DisplayName = "SecondsToReadableString Data Tests")]
        [InlineData(null, null)]
        [InlineData(0, null)]
        [InlineData(60, "1 minute")]
        [InlineData(3600, "1 hour")]
        [InlineData(1200, "20 minutes")]
        [InlineData(1228, "20 minutes")]
        [InlineData(4461, "1 hour 14 minutes")]
        [InlineData(6746, "1 hour 52 minutes")]
        [InlineData(7200, "2 hours")]
        [InlineData(7465, "2 hours 4 minutes")]
        [InlineData(8400, "2 hours 20 minutes")]
        public void SecondsToReadableStringDataTests(long? seconds, string result)
        {
            seconds.ToReadableString().Should().Be(result);
        }
    }
}