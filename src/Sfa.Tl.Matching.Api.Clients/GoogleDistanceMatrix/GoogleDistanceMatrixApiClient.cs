using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix
{
    public class GoogleDistanceMatrixApiClient : IGoogleDistanceMatrixApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly MatchingConfiguration _configuration;
        private readonly ILogger<GoogleDistanceMatrixApiClient> _logger;
        private readonly string _baseUrl;

        public GoogleDistanceMatrixApiClient(ILogger<GoogleDistanceMatrixApiClient> logger, HttpClient httpClient, MatchingConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            _baseUrl = _configuration.GoogleMapsApiBaseUrl.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IDictionary<int, JourneyInfoDto>> GetJourneyTimesAsync(string originPostcode, 
            IList<LocationDto> destinations, string travelMode, long arrivalTimeSeconds)
        {
            const int batchSize = 100; //Max client-side elements: 100
            var batches = CreateBatches(destinations, batchSize);
            var distanceSearchResults = new Dictionary<int, JourneyInfoDto>(batches.Count);
            
            foreach (var batch in batches)
            {
                var (_, value) = batch;
                var response = SearchBatchAsync(originPostcode, value, travelMode, arrivalTimeSeconds).GetAwaiter().GetResult();
                if (response != null)
                {
                    var batchResults = await BuildResultAsync(response, value);
                    foreach (var (i, journeyInfoDto) in batchResults)
                    {
                        if (!distanceSearchResults.ContainsKey(i))
                        {
                            distanceSearchResults.Add(i, journeyInfoDto);
                        }
                    }
                }
            }

            return distanceSearchResults;
        }
        
        private Task<IDictionary<int, JourneyInfoDto>> BuildResultAsync(GoogleJourneyTimeResponse response, IList<LocationDto> destinations)
        {
            var results = new Dictionary<int, JourneyInfoDto>(response.DestinationAddresses.Length);

            if (string.Compare(response.Status, "OK", StringComparison.OrdinalIgnoreCase) != 0)
            {
                var message = $"Failure response from google api - {response.Status}";
                _logger.LogError(message);
                throw new Exception(message);
            }

            var currentRow = response.Rows[0];
            for (var i = 0; i < response.DestinationAddresses.Length; i++)
            {
                var element = currentRow.Elements[i];

                try
                {
                    if (element.Status == "OK" && destinations[i].Id > 0)
                    {
                        results[destinations[i].Id] = new JourneyInfoDto
                        {
                            DestinationId = destinations[i].Id,
                            JourneyTime = element.Duration?.Value ?? 0,
                            JourneyDistance = element.Distance?.Value ?? 0
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding results from google distance matrix api - {ex}");
                    throw;
                }
            }

            return Task.FromResult((IDictionary<int, JourneyInfoDto>)results);
        }

        private Dictionary<int, IList<LocationDto>> CreateBatches(IList<LocationDto> venues, int batchSize)
        {
            var batches = new Dictionary<int, IList<LocationDto>>();
            var items = venues;
            var batchNo = 0;
            while (items.Any())
            {
                var batch = items.Take(batchSize);
                batches.Add(++batchNo, batch.ToList());
                items = items.Skip(batchSize).ToList();
            }

            return batches;
        }

        private async Task<GoogleJourneyTimeResponse> SearchBatchAsync(string origin, IList<LocationDto> destinations, string travelMode, long arrivalTimeSeconds)
        {
            try
            {
                var uriBuilder = new StringBuilder($@"{_baseUrl}/distancematrix/json?");

                uriBuilder.Append("units=imperial");
                uriBuilder.Append($"&origins={WebUtility.UrlEncode(origin)}");

                uriBuilder.Append($"&mode={travelMode}");
                uriBuilder.Append($"&arrival_time={arrivalTimeSeconds}");
                uriBuilder.Append("&destinations=");

                for (var i = 0; i < destinations.Count; i++)
                {
                    if (i > 0) uriBuilder.Append("%7C");

                    uriBuilder.Append($"{WebUtility.UrlEncode(destinations[i].Postcode)}");
                }

                uriBuilder.Append($"&key={_configuration.GoogleMapsApiKey}");

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var serializerSettings = new JsonSerializerSettings
                {
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    DateParseHandling = DateParseHandling.None,
                    Converters =
                    {
                        new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
                    }
                };

                return JsonConvert
                    .DeserializeObject<GoogleJourneyTimeResponse>(
                        jsonResponse, serializerSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failure calling google api - {ex}");
                throw;
            }
        }
    }
}
