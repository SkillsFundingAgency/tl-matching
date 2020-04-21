using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public string UtcNowString(string format)
        {
            return UtcNow().ToString(format).Replace("Z", string.Empty);
        }

        public DateTime MinValue()
        {
            return DateTime.MinValue;
        }

        public DateTime AddWorkingDays(DateTime startDate, TimeSpan timeSpan, IList<DateTime> holidays)
        {
            var endDate = startDate.AddMilliseconds(timeSpan.TotalMilliseconds);
            var direction = startDate < endDate ? 1 : -1;
            while (startDate.Date != endDate.Date)
            {
                startDate = startDate.AddDays(direction);
                if (startDate.DayOfWeek == DayOfWeek.Saturday ||
                    startDate.DayOfWeek == DayOfWeek.Sunday ||
                    IsHoliday(startDate, holidays))
                {
                    endDate = endDate.AddDays(direction);
                }
            }
            return endDate.Date.AddDays(1).AddSeconds(-1);
        }

        public bool IsHoliday(DateTime date, IList<DateTime> holidays)
        {
            return holidays != null &&
                   holidays.Any(hol => hol.Date == date.Date);
        }

        public DateTime GetNthWorkingDayDate(DateTime currentDate, int n, IList<DateTime> bankHolidays)
        {
            var year = currentDate.Year;
            var month = currentDate.Month;
            var day = 0;

            for (var workingDay = 1; workingDay <= n; workingDay++)
            {
                day++;
                var candidate = new DateTime(year, month, day);
                while (candidate.DayOfWeek == DayOfWeek.Saturday ||
                       candidate.DayOfWeek == DayOfWeek.Sunday ||
                       IsHoliday(candidate, bankHolidays))
                {
                    day++;
                    candidate = new DateTime(year, month, day);
                }
            }

            return new DateTime(year, month, day);
        }
    }
}