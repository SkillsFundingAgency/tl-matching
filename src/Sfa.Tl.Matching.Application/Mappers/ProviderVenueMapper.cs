using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderVenueMapper : Profile
    {
        public ProviderVenueMapper()
        {
            CreateMap<ProviderVenue, RemoveProviderVenueViewModel>()
                .ForMember(m => m.ProviderVenueId, config => config.MapFrom(s => s.Id));

            CreateMap<RemoveProviderVenueViewModel, ProviderVenue>()
                .ForMember(m => m.Id, config => config.MapFrom(s => s.ProviderVenueId))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<RemoveProviderVenueViewModel, ProviderVenue>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<RemoveProviderVenueViewModel, ProviderVenue>>())
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<ProviderVenue, ProviderVenueViewModel>()
                .ForMember(m => m.ProviderVenueId, config => config.MapFrom(s => s.Id))
                .ForMember(m => m.QualificationCount, config => config.MapFrom(s => s.ProviderQualification.Count))
                ;

            CreateMap<ProviderVenue, ProviderVenueDetailViewModel>()
                .ForMember(m => m.Qualifications, config => config.Ignore())
                .ForMember(m => m.IsFromAddVenue, config => config.Ignore())
                .ForMember(m => m.SubmitAction, config => config.Ignore())
                ;

            CreateMap<ProviderVenueDetailViewModel, ProviderVenue>()
                .ForMember(m => m.Id, config => config.MapFrom(s => s.Id))
                .ForMember(m => m.Name, config => config.MapFrom(s => s.Name))
                .ForMember(m => m.IsEnabledForReferral, config => config.MapFrom(s => s.IsEnabledForReferral))
                .ForMember(m => m.Source, config => config.MapFrom(s => s.Source))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<ProviderVenueDetailViewModel, ProviderVenue>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<ProviderVenueDetailViewModel, ProviderVenue>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<AddProviderVenueViewModel, ProviderVenue>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Name, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.County, config => config.Ignore())
                .ForMember(m => m.Location, config => config.Ignore())
                .ForMember(m => m.Longitude, config => config.Ignore())
                .ForMember(m => m.Latitude, config => config.Ignore())
                .ForMember(m => m.IsRemoved, config => config.Ignore())
                .ForMember(m => m.IsEnabledForReferral, config => config.Ignore())
                .ForMember(m => m.Provider, config => config.Ignore())
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.Referral, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<AddProviderVenueViewModel, ProviderVenue>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                ;
        }
    }
}