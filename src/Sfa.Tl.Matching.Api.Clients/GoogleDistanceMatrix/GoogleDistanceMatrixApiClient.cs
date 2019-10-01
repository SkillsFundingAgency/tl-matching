﻿using System;
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
        private static readonly object ListLocker = new object();

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

        public Task<IList<JourneyInfoDto>> GetJourneyTimesAsync(decimal latitude, decimal longitude, 
            IList<LocationDto> destinations, string travelMode)
        {
            const int batchSize = 100; //Max client-side elements: 100
            var batches = CreateBatches(destinations, batchSize);
            var distanceSearchResults = new List<JourneyInfoDto>(batches.Count);

            Parallel.ForEach(batches, batch =>
            {
                var (_, value) = batch;
                var response = SearchBatchAsync(latitude, longitude, value, travelMode).GetAwaiter().GetResult();
                if (response != null)
                {
                    var batchResults = BuildResultAsync(response, value).GetAwaiter().GetResult();
                    lock (ListLocker)
                    {
                        distanceSearchResults.AddRange(batchResults);
                    }
                }
            });

            return Task.FromResult<IList<JourneyInfoDto>>(distanceSearchResults);
        }

        private Task<IList<JourneyInfoDto>> BuildResultAsync(GoogleDistanceMatrixResponse response, IList<LocationDto> venues)
        {
            var results = new List<JourneyInfoDto>(response.DestinationAddresses.Length);

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
                    if (element.Status == "OK")
                    {
                        results.Add(new JourneyInfoDto
                        {
                            DestinationId = venues?[i].Id ?? -1,
                            TravelTime = element.Duration?.Value ?? 0,
                            TravelDistance = element.Distance?.Value ?? 0
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding results from google distance matrix api - {ex}");
                    throw;
                }
            }

            return Task.FromResult((IList<JourneyInfoDto>)results);
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

        private async Task<GoogleDistanceMatrixResponse> SearchBatchAsync(decimal latitude, decimal longitude, IList<LocationDto> destinations, string travelMode)
        {
            try
            {
                var uriBuilder = new StringBuilder($@"{_baseUrl}/distancematrix/json?");

                uriBuilder.Append("units=imperial");
                uriBuilder.Append($"&origins={latitude}%2C{longitude}");
                uriBuilder.Append($"&mode={travelMode}");
                uriBuilder.Append("&destinations=");

                uriBuilder.Append("enc:");
                var polyline = EncodePolyline(destinations);
                uriBuilder.Append(WebUtility.UrlEncode(polyline));
                uriBuilder.Append(":");
                
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
                    .DeserializeObject<GoogleDistanceMatrixResponse>(
                        jsonResponse, serializerSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failure calling google api - {ex}");
                throw;
            }
        }
        
        //Implementation from https://gist.github.com/shinyzhu/4617989
        public string EncodePolyline(IList<LocationDto> points)
        {
            var sb = new StringBuilder();

            var encodeDiff = (Action<int>)(diff =>
            {
                var shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;

                var rem = shifted;

                while (rem >= 0x20)
                {
                    sb.Append((char)((0x20 | (rem & 0x1f)) + 63));

                    rem >>= 5;
                }

                sb.Append((char)(rem + 63));
            });

            var lastLat = 0;
            var lastLng = 0;

            foreach (var point in points)
            {
                var lat = (int)Math.Round((double)point.Latitude * 1E5);
                var lng = (int)Math.Round((double)point.Longitude * 1E5);

                if (lat == lastLat || lng == lastLng)
                    continue;

                encodeDiff(lat - lastLat);
                encodeDiff(lng - lastLng);

                lastLat = lat;
                lastLng = lng;
            }
            return sb.ToString();
        }
    }
}
