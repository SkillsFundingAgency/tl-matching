using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
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

        public async Task<(bool, string)> IsValidPostCode(string postCode)
        {
            return await _locationService.IsValidPostCode(postCode);
        }

        public async Task<int> CreateVenue(ProviderVenueDto dto)
        {
            var providerVenue = _mapper.Map<ProviderVenue>(dto);

            var geoLocationData = await _locationService.GetGeoLocationData(dto.Postcode);
            providerVenue.Latitude = geoLocationData.Latitude.ToDecimal();
            providerVenue.Longitude = geoLocationData.Longitude.ToDecimal();
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            providerVenue.Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(geoLocationData.Longitude), double.Parse(geoLocationData.Latitude)));

            return await _providerVenueRepository.Create(providerVenue);
        }

        public async Task<ProviderVenueDetailViewModel> GetVenueWithQualifications(string postcode)
        {
            var viewModel = await _providerVenueRepository.GetVenueWithQualifications(postcode);

            return viewModel;
        }

        public async Task UpdateVenue(ProviderVenueDetailViewModel viewModel)
        {
            var trackedEntity = await _providerVenueRepository.GetSingleOrDefault(v => v.Id == viewModel.Id);
            trackedEntity = _mapper.Map(viewModel, trackedEntity);

            await _providerVenueRepository.Update(trackedEntity);
        }
    }
}