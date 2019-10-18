using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProximityService : IProximityService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IGoogleDistanceMatrixApiClient _googleDistanceMatrixApiClient;

        public ProximityService(ISearchProvider searchProvider,
            ILocationApiClient locationApiClient,
            IGoogleDistanceMatrixApiClient googleDistanceMatrixApiClient)
        {
            _searchProvider = searchProvider;
            _locationApiClient = locationApiClient;
            _googleDistanceMatrixApiClient = googleDistanceMatrixApiClient;
        }

        public async Task<IList<SearchResultsViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationApiClient.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximityAsync(dto);

            if (searchResults != null && searchResults.Any())
            {
                Debug.Write($"Search results for {dto.Postcode} route {dto.SelectedRouteId} - {searchResults.Count} results, ");
                searchResults = await FilterByJourneyTimeAsync(dto, searchResults);
                Debug.WriteLine($" {searchResults.Count} filtered results");
            }

            return searchResults ?? new List<SearchResultsViewModelItem>();
        }

        private async Task<IList<SearchResultsViewModelItem>> FilterByJourneyTimeAsync(ProviderSearchParametersDto searchParameters, IList<SearchResultsViewModelItem> searchResults)
        {
            var originPostcode = searchParameters.Postcode;
            var originLatitude = double.Parse(searchParameters.Latitude);
            var originLongitude = double.Parse(searchParameters.Longitude);

            var destinations = searchResults
                //.Where(v => v.Latitude != 0 && v.Longitude != 0)
                .Select(v => new LocationDto
                {
                    Id = v.ProviderVenueId,
                    Postcode = v.ProviderVenuePostcode,
                    Latitude = v.Latitude ?? 0,
                    Longitude = v.Longitude ?? 0
                }).ToList();

            var arrivalTimeSeconds = GetArrivalTime();

            var journeyResults =
                await Task.WhenAll(
                    _googleDistanceMatrixApiClient.GetJourneyTimesAsync(originPostcode, originLatitude, originLongitude, destinations, TravelMode.Driving, arrivalTimeSeconds),
                    _googleDistanceMatrixApiClient.GetJourneyTimesAsync(originPostcode, originLatitude, originLongitude, destinations, TravelMode.Transit, arrivalTimeSeconds));

            var journeyTimesByCar = journeyResults[0];
            var journeyTimesByPublicTransport = journeyResults[1];

            await Task.WhenAll(
                CheckAndMergeFailedResultsUsingLatLongSearch(destinations, journeyTimesByCar, originPostcode,
                    originLatitude, originLongitude, TravelMode.Driving, arrivalTimeSeconds),
                CheckAndMergeFailedResultsUsingLatLongSearch(destinations, journeyTimesByPublicTransport,
                    originPostcode, originLatitude, originLongitude, TravelMode.Transit, arrivalTimeSeconds));

            const int oneHour = 60 * 60;
            var results = searchResults
                .Where(s =>
                        journeyTimesByCar.Any(d => d.Key == s.ProviderVenueId &&
                                                   d.Value.JourneyTime < oneHour)
                        ||
                        journeyTimesByPublicTransport.Any(d => d.Key == s.ProviderVenueId &&
                                                               d.Value.JourneyTime < oneHour)
                    )
                .Select(r => new SearchResultsViewModelItem
                {
                    ProviderVenueTown = r.ProviderVenueTown,
                    ProviderVenuePostcode = r.ProviderVenuePostcode,
                    ProviderVenueId = r.ProviderVenueId,
                    ProviderName = r.ProviderName,
                    ProviderDisplayName = r.ProviderDisplayName,
                    ProviderVenueName = r.ProviderVenueName,
                    Distance = r.Distance,
                    IsTLevelProvider = r.IsTLevelProvider,
                    QualificationShortTitles = r.QualificationShortTitles,
                    JourneyTimeByPublicTransport = journeyTimesByPublicTransport.TryGetValue(r.ProviderVenueId, out var tVal)
                            ? tVal.JourneyTime : (long?)null,
                    JourneyTimeByCar = journeyTimesByCar.TryGetValue(r.ProviderVenueId, out var dVal)
                        ? dVal.JourneyTime : (long?)null
                }).OrderBy(r => r.Distance).ToList();

            return results;
        }

        private async Task CheckAndMergeFailedResultsUsingLatLongSearch(List<LocationDto> destinations, IDictionary<int, JourneyInfoDto> journeys, string originPostcode, double originLatitude, double originLongitude, string travelMode, long arrivalTimeSeconds)
        {
            var missingDestinations = destinations
                .Where(d => journeys.All(j => d.Id != j.Key))
                .ToList();

            if (missingDestinations.Any())
            {
                var journeyTimes =
                    await _googleDistanceMatrixApiClient.GetJourneyTimesAsync(originPostcode, originLatitude, originLongitude, missingDestinations, TravelMode.Driving, arrivalTimeSeconds, true);

                foreach (var newResult in journeyTimes)
                {
                    Debug.WriteLine(
                        $"{travelMode} - found extra journey {newResult.Key} - distance {newResult.Value.JourneyDistance} time {newResult.Value.JourneyTime}");
                    journeys.TryAdd(newResult.Key, newResult.Value);
                }
            }
        }

        public async Task<IList<SearchResultsByRouteViewModelItem>> SearchProvidersForOtherRoutesByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationApiClient.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersForOtherRoutesByPostcodeProximityAsync(dto);

            var results = searchResults.Any() ? searchResults : new List<SearchResultsByRouteViewModelItem>();

            return results;
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode)
        {
            return await _locationApiClient.IsValidPostcodeAsync(postcode, true);
        }

        private long GetArrivalTime()
        {
            var dateNextWednesday =
                DateTime.Now.AddDays(((int)DayOfWeek.Wednesday - (int)DateTime.Now.DayOfWeek + 7) % 7).Date;
            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            var hour = ukTimeZone.IsDaylightSavingTime(dateNextWednesday) ? 8 : 9;
            var nineAmWednesday = new DateTime(dateNextWednesday.Year, dateNextWednesday.Month, dateNextWednesday.Day,
                hour, 0, 0, DateTimeKind.Utc);

            //var convertBack = TimeZoneInfo.ConvertTime(nineAmWednesday, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            var arrivalTimeOffset = new DateTimeOffset(nineAmWednesday, TimeSpan.Zero);
            return arrivalTimeOffset.ToUnixTimeSeconds();
        }
    }
}