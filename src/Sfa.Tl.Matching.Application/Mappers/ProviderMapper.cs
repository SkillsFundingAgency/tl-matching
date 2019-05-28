using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderDto, Provider>()
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<Provider, ProviderDto>()
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                ;

            CreateMap<Provider, ProviderDetailViewModel>()
                .ForMember(m => m.SubmitAction, config => config.Ignore())
                .ForMember(m => m.ProviderVenues, config => config.MapFrom(s => s.ProviderVenue))
                ;

            CreateMap<ProviderDetailViewModel, Provider>()
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                .ForMember(m => m.Source, config => config.MapFrom<LoggedInUserEmailResolver<ProviderDetailViewModel, Provider>>())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<ProviderDetailViewModel, Provider>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<ProviderDetailViewModel, Provider>>())
                ;

            CreateMap<CreateProviderViewModel, Provider>()
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                .ForMember(m => m.Source, config => config.MapFrom<LoggedInUserEmailResolver<CreateProviderViewModel, Provider>>())
                .ForMember(m => m.DisplayName, config => config.Ignore())
                .ForMember(m => m.PrimaryContact, config => config.Ignore())
                .ForMember(m => m.PrimaryContactEmail, config => config.Ignore())
                .ForMember(m => m.PrimaryContactPhone, config => config.Ignore())
                .ForMember(m => m.SecondaryContact, config => config.Ignore())
                .ForMember(m => m.SecondaryContactEmail, config => config.Ignore())
                .ForMember(m => m.SecondaryContactPhone, config => config.Ignore())
                .ForMember(m => m.IsEnabledForReferral, config => config.Ignore())
                .ForMember(m => m.IsCdfProvider, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<CreateProviderViewModel, Provider>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<CreateProviderViewModel, Provider>>())
                ;

            CreateMap<Provider, ProviderSearchResultDto>();
            CreateMap<ProviderReference, ProviderSearchResultDto>();

            CreateMap<Provider, ProviderSearchResultItemViewModel>()
                .ForMember(m => m.ProviderId, config => config.MapFrom(s => s.Id))
                .ForMember(m => m.ProviderName, config => config.MapFrom(s => s.Name))
                .ForMember(m => m.IsCdfProvider, config => config.MapFrom(s => s.IsCdfProvider ? "Yes" : "No"))
                ;
        }
    }
}