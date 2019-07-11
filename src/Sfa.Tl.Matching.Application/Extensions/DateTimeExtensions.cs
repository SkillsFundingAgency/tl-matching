using System;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetTimeWithDate(this DateTime dateTime, string seperator)
        {
            var timeZoneDateTime = dateTime.ToGmtStandardTime();
            var timeWithDate = $"{timeZoneDateTime:hh:mm}{timeZoneDateTime.ToString("tt").ToLower()}" + 
                               $" {seperator} {timeZoneDateTime:dd MMMM yyyy}";

            return timeWithDate;
        }

        public static DateTime ToGmtStandardTime(this DateTime date)
        {
            var dateTimeWithKind = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(dateTimeWithKind, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        }
    }
}