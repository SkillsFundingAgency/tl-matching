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

        public DateTime? GetReferralDateAsync(IList<DateTime> bankHolidays, string timeSpan)
        {
            var employerFeedbackTimespan = TimeSpan.Parse(timeSpan);

            if (IsHoliday(UtcNow().Date, bankHolidays))
                return null;

            var referralDate = AddWorkingDays(UtcNow().Date, employerFeedbackTimespan, bankHolidays);

            return referralDate;
        }
    }
}