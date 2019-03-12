using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class UpdateProviderVenueProximityDataMapper : Profile
    {
        public UpdateProviderVenueProximityDataMapper()
        {
            CreateMap<SaveProximityData, ProviderVenue>()
                .ForMember(m => m.Location, o => o.MapFrom(s => GetLocation(s.Longitude, s.Latitude)))
                .ForMember(m => m.Latitude, o => o.MapFrom(s => s.Latitude.ToDecimal()))
                .ForMember(m => m.Longitude, o => o.MapFrom(s => s.Longitude.ToDecimal()))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver>())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => "System"))
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.Postcode, o => o.Ignore())
                .ForMember(m => m.Referral, o => o.Ignore())
                .ForMember(m => m.Source, o => o.Ignore())
                .ForMember(m => m.Town, o => o.Ignore())
                .ForMember(m => m.County, o => o.Ignore())
                .ForMember(m => m.Provider, o => o.Ignore())
                .ForMember(m => m.ProviderId, o => o.Ignore())
                .ForMember(m => m.ProviderQualification, o => o.Ignore())
                .ForMember(m => m.CreatedBy, o => o.Ignore())
                .ForMember(m => m.CreatedOn, o => o.Ignore())
                ;
        }

        public IPoint GetLocation(string longitude, string latitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            return geometryFactory.CreatePoint(new Coordinate(double.Parse(latitude), double.Parse(longitude)));
        }
    }
}