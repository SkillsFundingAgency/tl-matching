using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.BankHolidays
{
    public class BankHolidaysApiClient : IBankHolidaysApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly MatchingConfiguration _matchingConfiguration;

        public BankHolidaysApiClient(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _matchingConfiguration = matchingConfiguration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IList<BankHolidayResultDto>> GetBankHolidaysAsync()
        {
            var url = _matchingConfiguration.BankHolidaysJsonUrl;

            var responseMessage = await _httpClient.GetAsync(url);

            responseMessage.EnsureSuccessStatusCode();

            return await DeserializeBankHolidayList(responseMessage.Content);
        }

        private async Task<IList<BankHolidayResultDto>> DeserializeBankHolidayList(HttpContent content)
        {
            var jsonDoc = await JsonDocument.ParseAsync(await content.ReadAsStreamAsync());

            var allHolidays = new List<BankHolidayResultDto>();

            foreach (var holidayYear
                     in jsonDoc
                         .RootElement
                         .GetProperty("divisions")
                         .GetProperty("england-and-wales")
                         .EnumerateObject()
                         .Where(y => short.TryParse(y.Name, out _)))
            {
                var holidays = holidayYear.Value
                    .EnumerateArray()
                    .Select(x =>
                        new BankHolidayResultDto
                        {
                            Title = x.GetProperty("title").GetString(),//?.Replace("bank_holidays.", "").Replace('_', ' '),
                                                                       //DateString = x.GetString("date"),
                            Date = DateTime.TryParseExact(x.GetProperty("date").GetString(), "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt)
                                ? dt : DateTime.MinValue
                        })
                    .ToList();

                allHolidays.AddRange(holidays);
            }

            return allHolidays.OrderBy(h => h.Date).ToList();
        }
    }
}
