using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderVenueService : IProviderVenueService
    {
        private readonly IMapper _mapper;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IGoogleMapApiClient _googleMapApiClient;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public ProviderVenueService(IMapper mapper,
            IProviderVenueRepository providerVenueRepository,
            ILocationApiClient locationApiClient,
            IGoogleMapApiClient googleMapApiClient)
        {
            _mapper = mapper;
            _locationApiClient = locationApiClient;
            _googleMapApiClient = googleMapApiClient;
            _providerVenueRepository = providerVenueRepository;
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode)
        {
            var (valid, postcodeResult) = await _locationApiClient.IsValidPostcodeAsync(postcode);

            if (!valid)
                (valid, postcodeResult) = await _locationApiClient.IsTerminatedPostcodeAsync(postcode);

            return (valid, postcodeResult);
        }

        public async Task<ProviderVenueDetailViewModel> GetVenueAsync(int providerId, string postcode)
        {
            var venue = await _providerVenueRepository.GetSingleOrDefaultAsync(pv => pv.ProviderId == providerId && pv.Postcode == postcode);

            var dto = venue == null ? null : _mapper.Map<ProviderVenue, ProviderVenueDetailViewModel>(venue);

            return dto;
        }

        public async Task<int> CreateVenueAsync(AddProviderVenueViewModel viewModel)
        {
            var providerVenue = _mapper.Map<ProviderVenue>(viewModel);

            await GetGeoLocationDataAsync(viewModel, providerVenue);

            await GetGoogleAddressDetailsAsync(providerVenue);

            if (string.IsNullOrWhiteSpace(providerVenue.Name))
            {
                providerVenue.Name = providerVenue.Postcode;
            }

            return await _providerVenueRepository.CreateAsync(providerVenue);
        }

        private async Task GetGoogleAddressDetailsAsync(ProviderVenue providerVenue)
        {
            providerVenue.Town = await _googleMapApiClient.GetAddressDetailsAsync(providerVenue.Postcode);
        }

        private async Task GetGeoLocationDataAsync(AddProviderVenueViewModel viewModel, ProviderVenue providerVenue)
        {
            var geoLocationData = await _locationApiClient.GetGeoLocationDataAsync(viewModel.Postcode, true);

            providerVenue.Postcode = geoLocationData.Postcode;
            providerVenue.Latitude = geoLocationData.Latitude.ToDecimal();
            providerVenue.Longitude = geoLocationData.Longitude.ToDecimal();

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            providerVenue.Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(geoLocationData.Longitude), double.Parse(geoLocationData.Latitude)));
        }

        public async Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(int providerVenueId)
        {
            var viewModel = await _providerVenueRepository.GetVenueWithQualificationsAsync(providerVenueId);

            return viewModel;
        }

        public async Task UpdateVenueAsync(ProviderVenueDetailViewModel viewModel)
        {
            var trackedEntity = await _providerVenueRepository.GetSingleOrDefaultAsync(v => v.Id == viewModel.Id);

            trackedEntity = _mapper.Map(viewModel, trackedEntity);

            await _providerVenueRepository.UpdateAsync(trackedEntity);
        }

        public async Task UpdateVenueAsync(RemoveProviderVenueViewModel viewModel)
        {
            var providerVenue = _mapper.Map<RemoveProviderVenueViewModel, ProviderVenue>(viewModel);
            providerVenue.IsRemoved = true;

            await _providerVenueRepository.UpdateWithSpecifiedColumnsOnlyAsync(providerVenue,
                x => x.IsRemoved,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        public async Task<RemoveProviderVenueViewModel> GetRemoveProviderVenueViewModelAsync(int providerVenueId)
        {
            var providerVenue = await _providerVenueRepository.GetSingleOrDefaultAsync(p => p.Id == providerVenueId);
            return _mapper.Map<ProviderVenue, RemoveProviderVenueViewModel>(providerVenue);
        }

        public async Task<string> GetVenuePostcodeAsync(int providerVenueId)
        {
            var providerVenue = await _providerVenueRepository.GetSingleOrDefaultAsync(p => p.Id == providerVenueId);
            return providerVenue.Postcode;
        }
    }
}