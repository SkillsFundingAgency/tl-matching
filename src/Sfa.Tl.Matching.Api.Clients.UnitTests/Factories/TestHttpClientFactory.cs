using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public abstract class TestHttpClientFactory
    {
        protected HttpClient CreateClient(object response, string uri, string contentType = "application/json")
        {
            var serializedResponse = JsonSerializer.Serialize(response);
            return CreateClient(serializedResponse, uri, contentType);
        }

        protected HttpClient CreateClient(string serializedResponse, string uri, string contentType = "application/json")
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serializedResponse)
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