using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.Extensions;
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
            var jsonDocument = await JsonDocument.ParseAsync(await content.ReadAsStreamAsync());
            return jsonDocument
                .RootElement
                .EnumerateObject()
                .Single(s => s.Name == "england-and-wales")
                .Value
                .GetProperty("events")
                .EnumerateArray()
                .Select(x =>
                    new BankHolidayResultDto
                    {
                        Title = x.SafeGetString("title")?
                            .Replace("bank_holidays.", "")
                            .Replace('_', ' '),
                        Date = DateTime.TryParseExact(x.SafeGetString("date"),
                            "yyyy-MM-dd",
                            CultureInfo.CurrentCulture,
                            DateTimeStyles.None, out var dt)
                            ? dt : DateTime.MinValue
                    })
                .OrderBy(h => h.Date)
                .ToList();
        }
    }
}
