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
                .ForMember(m => m.ProviderId, o => o.MapFrom(s => s.ProviderId))
                ;
        }
    }
}