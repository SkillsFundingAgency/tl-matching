using System;

namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class ProviderDisplayExtensions
    {
        public static string GetDisplayText(string venueName, string postcode, string displayName, bool includePartOf = true)
        {
            return string.Compare(venueName, postcode, StringComparison.InvariantCultureIgnoreCase) == 0
                ? $"{displayName} ({postcode})"
                : includePartOf ? 
                    $"{venueName} part of {displayName} ({postcode})"
                    : $"{venueName} ({postcode})" ;
        }

        public static string GetProviderEmailDisplayText(string venueName, string postcode, string displayName)
        {
            return string.Compare(venueName, postcode, StringComparison.InvariantCultureIgnoreCase) == 0
                ? $"{displayName}"
                : $"{venueName}";
        }

        public static string GetProvideReportDisplayText(string venueName, string postcode, string displayName, bool includePartOf = true)
        {
            return string.Compare(venueName, postcode, StringComparison.InvariantCultureIgnoreCase) == 0
                ? $"{displayName}"
                : includePartOf ?
                    $"{venueName} (part of {displayName})"
                    : $"{venueName}";
        }
    }
}