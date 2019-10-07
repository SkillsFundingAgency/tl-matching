using System;
using Humanizer;
using Humanizer.Localisation;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadableString(this long? seconds, int precision = 2, TimeUnit minUnit = TimeUnit.Minute, string separator = " ")
        {
            if (!seconds.HasValue || seconds.Value == 0)
                return null;

            return TimeSpan.FromSeconds(seconds.Value)
                .RoundToNearestMinute()
                .Humanize(precision,
                    minUnit: minUnit,
                    collectionSeparator: separator);
        }

        public static TimeSpan RoundToNearestMinute(this TimeSpan ts)
        {
            var roundedMinutes = Math.Round(ts.TotalMinutes, MidpointRounding.AwayFromZero);
            return TimeSpan.FromMinutes(roundedMinutes);
        }
    }
}