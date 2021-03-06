using System.Net.Http;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class TerminatedPostcodesTestHttpClientFactory : TestHttpClientFactory
    {
        public HttpClient Get(string requestPostcode, PostcodeLookupResultDto responseData)
        {
            var response = new PostcodeLookupResponse
            {
                Result = responseData,
                Status = "OK"
            };

            return CreateClient(response, $"https://example.com/terminated_postcodes/{requestPostcode.Replace(" ", "")}");
        }
    }
}