using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class GoogleMapsTestHttpClientFactory : TestHttpClientFactory
    {
        public HttpClient Get(string requestPostcode = "CV12WT", string responseTown = "Coventry")
        {
            var responseJson = GetTestJson();

            return CreateClient(responseJson,
                "https://example.com/place/textsearch/json?region=uk&radius=1&key=TEST_KEY&query=CV12WT");
        }

        public HttpClient GetWithBadStatus(string requestPostcode = "CV12WT", string responseTown = "Coventry")
        {
            var responseJson = GetTestJson("TestGooglePlacesResultWithNotFoundStatus");

            return CreateClient(responseJson,
                "https://example.com/place/textsearch/json?region=uk&radius=1&key=TEST_KEY&query=CV12WT");
        }

        private static string GetTestJson(string fileName = "TestGooglePlacesResult")
        {
            var resourceName = $"{typeof(GoogleMapsTestHttpClientFactory).Namespace}.Data.{fileName}.json";
            using var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(templateStream ?? throw new InvalidOperationException("Could not find json test file"));
            return streamReader.ReadToEnd();
        }
    }
}