using System.Text.Json;

namespace Sfa.Tl.Matching.Api.Clients.Extensions
{
    public static class JsonExtensions
    {
        public static double SafeGetDouble(this JsonElement element, string propertyName, double defaultValue = default)
        {
            return element.TryGetProperty(propertyName, out var property)
                   && property.ValueKind == JsonValueKind.Number
                   && property.TryGetDouble(out var val)
                ? val
                : defaultValue;
        }

        public static string SafeGetString(this JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var property)
                   && property.ValueKind == JsonValueKind.String
                ? property.GetString()
                : default;
        }
    }
}
