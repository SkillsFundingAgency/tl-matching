using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.TestClients
{
    public class TestPostcodesIoHttpClient
    {
        private readonly bool _isMockedHttpClient;

        public TestPostcodesIoHttpClient()
        {
            _isMockedHttpClient = TestConfiguration.IsMockedHttpClient;
        }

        public HttpClient Get(string requestPostcode = "CV1 2WT", string responsePostcode = "CV1 2WT")
        {
            if (_isMockedHttpClient)
                return GetMockedHttpClient(requestPostcode, responsePostcode);

            return new HttpClient();
        }

        private static HttpClient GetMockedHttpClient(string requestPostcode, string responsePostcode)
        {
            var response = new PostcodeLookupResponse
            {
                Result = new PostcodeLookupResultDto
                {
                    Latitude = "52.400997",
                    Longitude = "-1.508122",
                    Postcode = responsePostcode
                }
            };
            var serialised = JsonConvert.SerializeObject(response);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serialised)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri($"https://api.postcodes.io/postcodes/{requestPostcode.ToLetterOrDigit()}"),
                httpResponseMessage);

            var httpClient = new HttpClient(fakeResponseHandler);

            return httpClient;
        }
    }
}