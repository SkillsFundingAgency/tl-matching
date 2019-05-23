﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class PostcodesIoHttpClient
    {
        public HttpClient Get(string requestPostcode = "CV1 2WT", string responsePostcode = "CV1 2WT")
        {
            var response = new PostCodeLookupResponse
            {
                result = new PostCodeLookupResultDto
                {
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