﻿using AutoMapper;
using Sfa.Tl.Matching.Application.Extensions;
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
            CreateMap<Provider, ProviderDetailViewModel>()
                .ForMember(m => m.SubmitAction, config => config.Ignore())
                .ForMember(m => m.ProviderVenues, config => config.MapFrom(s => s.ProviderVenue))
                ;

            CreateMap<ProviderDetailViewModel, Provider>()
                .ForMember(m => m.DisplayName, config => config.MapFrom(s => s.DisplayName.ToTitleCase()))
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                .ForMember(m => m.Source, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.ProviderFeedbackSentOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<ProviderDetailViewModel, Provider>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<ProviderDetailViewModel, Provider>>())
                ;

            CreateMap<CreateProviderDetailViewModel, Provider>()
                .ForMember(m => m.DisplayName, config => config.MapFrom(s => s.DisplayName.ToTitleCase()))
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.ProviderFeedbackSentOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<CreateProviderDetailViewModel, Provider>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                ;

            CreateMap<Provider, ProviderSearchResultDto>();
            CreateMap<ProviderReference, ProviderSearchResultDto>();
            CreateMap<Provider, ProviderSearchResultItemViewModel>()
                .ForMember(m => m.IsCdfProvider, config => config.MapFrom(s => s.IsCdfProvider ? "Yes" : "No"));

            CreateMap<UsernameForFeedbackSentDto, Provider>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.ProviderFeedbackSentOn, config => config.MapFrom<UtcNowResolver<UsernameForFeedbackSentDto, Provider>>())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.Username))
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<UsernameForFeedbackSentDto, Provider>>())
                .ForMember(m => m.UkPrn, config => config.Ignore())
                .ForMember(m => m.Name, config => config.Ignore())
                .ForMember(m => m.DisplayName, config => config.Ignore())
                .ForMember(m => m.OfstedRating, config => config.Ignore())
                .ForMember(m => m.PrimaryContact, config => config.Ignore())
                .ForMember(m => m.PrimaryContactEmail, config => config.Ignore())
                .ForMember(m => m.PrimaryContactPhone, config => config.Ignore())
                .ForMember(m => m.SecondaryContact, config => config.Ignore())
                .ForMember(m => m.SecondaryContactEmail, config => config.Ignore())
                .ForMember(m => m.SecondaryContactPhone, config => config.Ignore())
                .ForMember(m => m.IsEnabledForReferral, config => config.Ignore())
                .ForMember(m => m.IsCdfProvider, config => config.Ignore())
                .ForMember(m => m.IsTLevelProvider, config => config.Ignore())
                .ForMember(m => m.Source, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.ProviderVenue, config => config.Ignore())
                ;
        }
    }
}