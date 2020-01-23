using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Tests.Common.HttpClient
{
    public abstract class TestHttpClientFactory
    {
        protected System.Net.Http.HttpClient CreateMockClient(object response, string uri, string contentType = "application/json", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var serialised = JsonConvert.SerializeObject(response);

            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(serialised, Encoding.UTF8, contentType)
            };

            var fakeMessageHandler = new FakeHttpMessageHandler();
            fakeMessageHandler.AddFakeResponse(new Uri(uri), httpResponseMessage);

            var httpClient = new System.Net.Http.HttpClient(fakeMessageHandler);

            return httpClient;
        }
    }
}