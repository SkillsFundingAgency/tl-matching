using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
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
        private readonly ILocationService _locationService;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public ProviderVenueService(IMapper mapper,
            IRepository<ProviderVenue> providerVenueRepository,
            ILocationService locationService)
        {
            _mapper = mapper;
            _locationService = locationService;
            _providerVenueRepository = (IProviderVenueRepository)providerVenueRepository;
        }

        public async Task<(bool, string)> IsValidPostCodeAsync(string postCode)
        {
            return await _locationService.IsValidPostCode(postCode);
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

            var geoLocationData = await _locationService.GetGeoLocationData(viewModel.Postcode);
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

        public async Task<HideProviderVenueViewModel> GetHideProviderVenueViewModelAsync(int venueId)
        {
            var providerVenue = await _providerVenueRepository.GetSingleOrDefault(p => p.Id == venueId);
            return _mapper.Map<ProviderVenue, HideProviderVenueViewModel>(providerVenue);
        }

        public async Task SetIsProviderVenueEnabledForSearchAsync(int providerVenueId, bool isEnabled)
        {
            //TODO: Use specific map with resolver. Use HideProviderViewModel viewModel for parameter
            var providerVenue = await _providerVenueRepository.GetSingleOrDefault(p => p.Id == providerVenueId);
            if (providerVenue != null)
            {
                providerVenue.IsEnabledForSearch = isEnabled;
                await _providerVenueRepository.Update(providerVenue);
            }
        }
    }
}