using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

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
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.Location, config => config.Ignore())
                ;

            CreateMap<ProviderVenue, ProviderVenueViewModel>()
                .ForMember(m => m.ProviderVenueId, config => config.MapFrom(s => s.Id))
                .ForMember(m => m.QualificationCount, config => config.MapFrom(s => s.ProviderQualification.Count));
        }
    }
}