namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class ProviderDisplayExtensions
    {
        public static string GetDisplayText(string venueName, string postcode, string displayName)
        {
            return venueName == postcode
                ? displayName
                : $"{venueName} (part of {displayName})";
        }
    }
}