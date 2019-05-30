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

        public string UtcNowString(string format)
        {
            return UtcNow().ToString(format).Replace("Z", string.Empty);
        }

        public DateTime MinValue()
        {
            return DateTime.MinValue;
        }
    }
}