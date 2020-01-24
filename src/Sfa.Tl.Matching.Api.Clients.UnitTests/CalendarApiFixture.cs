using System;
using System.IO;
using System.Reflection;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class CalendarApiFixture : TestHttpClientFactory
    {
        public ICalendarApiClient CalendarApiClient;

        public void GetCalendarApiClient()
        {
            var httpClient = CreateMockClient(GetTestJson(), "https://raw.githubusercontent.com/alphagov/calendars/master/lib/data/bank-holidays.json");
            CalendarApiClient = new CalendarApiClient(httpClient, new MatchingConfiguration
            {
                CalendarJsonUrl = "https://raw.githubusercontent.com/alphagov/calendars/master/lib/data/bank-holidays.json"
            });
        }

        private static string GetTestJson()
        {
            var resourceName = $"{typeof(CalendarApiFixture).Namespace}.TestBankHolidays.json";
            using (var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var streamReader = new StreamReader(templateStream ?? throw new InvalidOperationException("Could not find json test file")))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}