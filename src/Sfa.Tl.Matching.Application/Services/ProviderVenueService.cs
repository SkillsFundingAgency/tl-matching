using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderVenueService : IProviderVenueService
    {
        private readonly IMapper _mapper;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public ProviderVenueService(IMapper mapper,
            IRepository<ProviderVenue> providerVenueRepository,
            ILocationApiClient locationApiClient)
        {
            _mapper = mapper;
            _locationApiClient = locationApiClient;
            _providerVenueRepository = (IProviderVenueRepository)providerVenueRepository;
        }

        public async Task<(bool, string)> IsValidPostCodeAsync(string postCode)
        {
            return await _locationApiClient.IsValidPostCode(postCode);
        }

        public async Task<ProviderVenueDetailViewModel> GetVenue(int providerId, string postCode)
        {
            var venue = await _providerVenueRepository.GetSingleOrDefault(pv => pv.ProviderId == providerId &&
                                                                                pv.Postcode == postCode);

            var dto = venue != null
                ? _mapper.Map<ProviderVenue, ProviderVenueDetailViewModel>(venue)
                : null;

            return dto;
        }

        public async Task<int> CreateVenueAsync(AddProviderVenueViewModel viewModel)
        {
            var providerVenue = _mapper.Map<ProviderVenue>(viewModel);

            var geoLocationData = await _locationApiClient.GetGeoLocationData(viewModel.Postcode);
            providerVenue.Latitude = geoLocationData.Latitude.ToDecimal();
            providerVenue.Longitude = geoLocationData.Longitude.ToDecimal();
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            providerVenue.Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(geoLocationData.Longitude), double.Parse(geoLocationData.Latitude)));

            return await _providerVenueRepository.Create(providerVenue);
        }

        public async Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(int providerVenueId)
        {
            var viewModel = await _providerVenueRepository.GetVenueWithQualifications(providerVenueId);

            return viewModel;
        }

        public async Task UpdateVenueAsync(ProviderVenueDetailViewModel viewModel)
        {
            var trackedEntity = await _providerVenueRepository.GetSingleOrDefault(v => v.Id == viewModel.Id);

            trackedEntity = _mapper.Map(viewModel, trackedEntity);

            await _providerVenueRepository.Update(trackedEntity);
        }

        public async Task UpdateVenueAsync(RemoveProviderVenueViewModel viewModel)
        {
            var providerVenue = _mapper.Map<RemoveProviderVenueViewModel, ProviderVenue>(viewModel);
            providerVenue.IsRemoved = true;

            await _providerVenueRepository.UpdateWithSpecifedColumnsOnly(providerVenue,
                x => x.IsRemoved,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        public async Task<RemoveProviderVenueViewModel> GetRemoveProviderVenueViewModelAsync(int providerVenueId)
        {
            var providerVenue = await _providerVenueRepository.GetSingleOrDefault(p => p.Id == providerVenueId);
            return _mapper.Map<ProviderVenue, RemoveProviderVenueViewModel>(providerVenue);
        }

        public async Task<string> GetVenuePostcodeAsync(int providerVenueId)
        {
            var providerVenue = await _providerVenueRepository.GetSingleOrDefault(p => p.Id == providerVenueId);
            return providerVenue.Postcode;
        }
    }
}