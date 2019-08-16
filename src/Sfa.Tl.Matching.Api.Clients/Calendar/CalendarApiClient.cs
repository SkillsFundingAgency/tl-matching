using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.Calendar
{
    public class CalendarApiClient : ICalendarApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly MatchingConfiguration _matchingConfiguration;

        public CalendarApiClient(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _matchingConfiguration = matchingConfiguration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IList<BankHolidayResultDto>> GetBankHolidaysAsync()
        {
            var url = _matchingConfiguration.CalendarJsonUrl;

            var responseMessage = await _httpClient.GetAsync(url);

            responseMessage.EnsureSuccessStatusCode();

            using (var stream = await responseMessage.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var json = await JObject.LoadAsync(jsonReader);
                var englandAndWalesHolidays = json
                    .SelectTokens(
                        "$.divisions.england-and-wales..[?(@.date)]");

                var serializer = new JsonSerializer
                {
                    DateFormatString = "dd/MM/yyyy"
                };

                var allResults = englandAndWalesHolidays
                    .Select(x => x.ToObject<BankHolidayResultDto>(serializer))
                    .ToList();
                return allResults;
            }
        }
    }
}
