using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class ProviderVenueDtoMapper : Profile
    {
        public ProviderVenueDtoMapper()
        {
            CreateMap<ProviderVenueAddViewModel, ProviderVenueDto>()
                .ForMember(m => m.CreatedBy,
                    o => o.MapFrom<LoggedInUserNameResolver<ProviderVenueAddViewModel, ProviderVenueDto>>())
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForAllOtherMembers(config => config.Ignore());
            ;

            CreateMap<ProviderVenueDetailViewModel, UpdateProviderVenueDto>()
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<ProviderVenueDetailViewModel, UpdateProviderVenueDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<ProviderVenueDetailViewModel, UpdateProviderVenueDto>>())
                .ForMember(m => m.Name, o => o.MapFrom(s => s.VenueName))
                .ForAllOtherMembers(config => config.Ignore());
            ;
        }
    }
}