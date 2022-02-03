using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class BankHolidaysTestHttpClientFactory : TestHttpClientFactory
    {
        public string Url => "https://www.gov.uk/bank-holidays.json";

        public HttpClient Get(string responseJson)
        {
            var json = string.IsNullOrEmpty(responseJson)
                ? GetTestJson()
                : responseJson;
            
            return CreateClient(json, Url);
        }

        private static string GetTestJson()
        {
            var resourceName = $"{typeof(BankHolidaysTestHttpClientFactory).Namespace}.Data.TestBankHolidays.json";
            using var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(templateStream ?? throw new InvalidOperationException("Could not find json test file"));
            return streamReader.ReadToEnd();
        }
    }
}