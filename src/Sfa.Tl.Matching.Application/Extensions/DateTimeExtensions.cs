using System;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetTimeWithDate(this DateTime dateTime, string seperator)
        {
            var timeWithDate = $"{dateTime.ToLocalTime():hh:mm}{dateTime.ToString("tt").ToLower()}" +
                              $" {seperator} {dateTime:dd MMMM yyyy}";

            return timeWithDate;
        }
    }
}