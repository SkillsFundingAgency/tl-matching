using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class HttpClientFactory
    {
        public HttpClient Get(string requestPostCode = "CV12WT", string responseTown = "Coventry")
        {
            var response = new GooglePlacesResult
            {
                Results = new[]{ new Result
                {
                    FormattedAddress = $"Test Street, {responseTown} {requestPostCode}"
                }},
                Status = "OK"
            };

            var serialised = JsonConvert.SerializeObject(response);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serialised)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var fakeMessageHandler = new FakeHttpMessageHandler();
            fakeMessageHandler.AddFakeResponse(new Uri("https://example.com/place/textsearch/json?region=uk&radius=1&key=TEST_KEY&query=CV12WT"),
                httpResponseMessage);

            var httpClient = new HttpClient(fakeMessageHandler);

            return httpClient;
        }
    }
}