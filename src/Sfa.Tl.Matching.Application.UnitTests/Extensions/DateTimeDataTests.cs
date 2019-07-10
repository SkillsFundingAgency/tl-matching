//using System;
//using System.Collections.Generic;
//using FluentAssertions;
//using Sfa.Tl.Matching.Application.Extensions;
//using Xunit;

//namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
//{
//    [Trait("DateTime", "Data Tests")]
//    public class DateTimeExtensionsTests
//    {
//        public static IEnumerable<object[]> Data =>
//            new List<object[]>
//            {
//                new object[] { new DateTime(2000, 1, 1, 16, 50, 55, DateTimeKind.Utc), "on", "04:50pm on 01 January 2000" },
//                new object[] { new DateTime(2017, 4, 1, 22, 22, 3, DateTimeKind.Utc), "on", "11:22pm on 01 April 2017" },
//                new object[] { new DateTime(2018, 8, 18, 6, 22, 45, DateTimeKind.Utc), "on", "07:22am on 18 August 2018" },
//                new object[] { new DateTime(2019, 12, 31, 13, 02, 16, DateTimeKind.Utc), "on", "01:02pm on 31 December 2019" }
//            };

//        [Theory(DisplayName = "GetTimeWithDate Data Tests")]
//        [MemberData(nameof(Data))]
//        public void GetTimeWithDateDataTests(DateTime dateTime, string seperator, string result)
//        {
//            dateTime.GetTimeWithDate(seperator).Should().Be(result);
//        }
//    }
//}