using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class ProviderVenueDtoMapper : Profile
    {
        public ProviderVenueDtoMapper()
        {
            CreateMap<AddProviderVenueViewModel, ProviderVenueDto>()
                .ForMember(m => m.CreatedBy,
                    o => o.MapFrom<LoggedInUserNameResolver<AddProviderVenueViewModel, ProviderVenueDto>>())
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.Source, o => o.MapFrom(s => s.Source))
                .ForMember(m => m.ProviderId, o => o.MapFrom(s => s.ProviderId))
                .ForAllOtherMembers(config => config.Ignore());
            ;

            CreateMap<ProviderVenueDetailViewModel, UpdateProviderVenueDto>()
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<ProviderVenueDetailViewModel, UpdateProviderVenueDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<ProviderVenueDetailViewModel, UpdateProviderVenueDto>>())
                .ForMember(m => m.Name, o => o.MapFrom(s => s.VenueName))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.ProviderId, o => o.MapFrom(s => s.ProviderId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.Source, o => o.MapFrom(s => s.Source))
                .ForAllOtherMembers(config => config.Ignore());
            ;
        }
    }
}