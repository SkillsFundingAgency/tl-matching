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

        public DateTime AddWorkingDays(DateTime date, int days, IList<DateTime> holidays)
        {
            var direction = days < 0 ? -1 : 1;
            var newDate = date;
            while (days != 0)
            {
                newDate = newDate.AddDays(direction);
                if (newDate.DayOfWeek != DayOfWeek.Saturday &&
                    newDate.DayOfWeek != DayOfWeek.Sunday &&
                    !IsHoliday(newDate, holidays))
                {
                    days -= direction;
                }
            }
            return newDate;
        }

        private static bool IsHoliday(DateTime date, IList<DateTime> holidays)
        {
            return holidays != null && 
                   holidays.Any(hol => hol.Date == date.Date);
        }
    }
}