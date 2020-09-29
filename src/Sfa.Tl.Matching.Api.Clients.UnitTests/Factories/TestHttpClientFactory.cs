using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public abstract class TestHttpClientFactory
    {
        protected HttpClient CreateClient(object response, string uri, string contentType = "application/json")
        {
            var serialized = JsonConvert.SerializeObject(response);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serialized)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var fakeMessageHandler = new FakeHttpMessageHandler();
            fakeMessageHandler.AddFakeResponse(new Uri(uri),
                httpResponseMessage);

            var httpClient = new HttpClient(fakeMessageHandler);

            return httpClient;
        }
    }
}