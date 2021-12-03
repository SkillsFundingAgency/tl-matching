using System.Net.Http;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class PostcodesTestHttpClientFactory : TestHttpClientFactory
    {
        public HttpClient Get(string postcode, double? latitude, double? longitude)
        {
            var latitudestring = latitude?.ToString() ?? "null";
            var json =
                "{ " +
                "  \"status\": \"OK\"," +
                "  \"result\": { " +
               $"    \"postcode\":  \"{postcode}\"," +
               $"    \"latitude\":  {latitude?.ToString() ?? "null"}, " +
               $"    \"longitude\":  {longitude?.ToString() ?? "null"} " +
               "  }" +
               "}";

            return CreateClient(json, $"https://example.com/postcodes/{postcode.Replace(" ", "")}");
        }
    }
}