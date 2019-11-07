namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class ProviderDisplayExtensions
    {
        public static string GetDisplayText(string venueName, string postcode, string displayName, bool includePartOf = true)
        {
            return venueName == postcode
                ? $"{displayName} ({postcode})"
                : includePartOf ? 
                    $"{venueName} part of {displayName} ({postcode})"
                    : $"{venueName} ({postcode})" ;
        }

        public static string GetProviderEmailDisplayText(string venueName, string postcode, string displayName)
        {
            return venueName == postcode
                ? $"{displayName}"
                : $"{venueName}";
        }

    }
}