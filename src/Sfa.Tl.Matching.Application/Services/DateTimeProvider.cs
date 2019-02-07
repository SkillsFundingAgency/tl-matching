using System;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }

        public string UtcNowString(string format)
        {
            return UtcNow().ToString(format).Replace("Z", string.Empty);
        }

        public string NowString(string format)
        {
            return Now().ToString(format).Replace("Z", string.Empty);
        }
    }
}