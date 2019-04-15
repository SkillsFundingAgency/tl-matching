using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderVenueMapper : Profile
    {
        public ProviderVenueMapper()
        {
            CreateMap<ProviderVenueDto, ProviderVenue>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Provider, config => config.Ignore())
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.Referral, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.Location, config => config.Ignore())
                ;

            CreateMap<UpdateProviderVenueDto, ProviderVenue>()
                .ForMember(m => m.Name, o => o.MapFrom(s => s.Name))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());


            //CreateMap<ProviderVenueDetailViewModel, ProviderVenue>()
            //    .ForMember(m => m.Name, o => o.MapFrom(s => s.VenueName))
            //    .ForAllOtherMembers(config => config.Ignore());

            //CreateMap<ProviderVenueAddViewModel, ProviderVenue>()
            //    .ForMember(m => m.ProviderId, o => o.MapFrom(s => s.ProviderId))
            //    .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
            //    .ForMember(m => m.Source, o => o.MapFrom(s => s.Source))
            //    .ForAllOtherMembers(config => config.Ignore());
        }
    }
}