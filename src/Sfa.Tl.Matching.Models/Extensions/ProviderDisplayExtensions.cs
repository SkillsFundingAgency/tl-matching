namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class ProviderDisplayExtensions
    {
        public static string GetDisplayText(string venueName, string postcode, string displayName, bool includePartOf = true)
        {
            return venueName == postcode
                ? displayName
                : includePartOf ? 
                    $"{venueName} (part of {displayName})"
                    : venueName;
        }
    }
}