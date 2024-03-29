﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.IntegrationTests.BankHoliday
{
    public class TestBankHolidaysHttpClient
    {
        private readonly bool _isMockedHttpClient;

        public TestBankHolidaysHttpClient()
        {
            _isMockedHttpClient = TestConfiguration.IsMockedHttpClient;
        }

        public HttpClient Get(string responseData = "")
        {
            if (_isMockedHttpClient)
                return GetMockedHttpClient(responseData);

            return new HttpClient();
        }

        private static HttpClient GetMockedHttpClient(string responseData = null)
        {
            var json = string.IsNullOrEmpty(responseData)
                ? GetTestJson()
                : responseData;

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri("https://www.gov.uk/bank-holidays.json"),
                httpResponseMessage);

            var httpClient = new HttpClient(fakeResponseHandler);

            return httpClient;
        }

        private static string GetTestJson()
        {
            var resourceName = $"{typeof(TestBankHolidaysHttpClient).Namespace}.TestBankHolidays.json";
            using var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(templateStream ?? throw new InvalidOperationException("Could not find json test file"));
            return streamReader.ReadToEnd();
        }
    }
}