using System;
using System.Linq;
using AutoMapper;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderVenueQualificationMapper : Profile
    {
        public ProviderVenueQualificationMapper()
        {
            CreateMap<ProviderVenueQualificationFileImportDto, ProviderVenueQualificationDto>()
                .ForMember(m => m.UkPrn, o => o.MapFrom(s => s.UkPrn.ToLong()))
                .ForMember(m => m.InMatchingService, o => o.MapFrom(s => s.InMatchingService.ToBool()))
                .ForMember(m => m.IsCdfProvider, o => o.MapFrom(s => s.IsCdfProvider.ToBool()))
                .ForMember(m => m.IsEnabledForReferral, o => o.MapFrom(s => s.IsEnabledForReferral.ToBool()))
                .ForMember(m => m.VenueIsEnabledForReferral, o => o.MapFrom(s => s.VenueIsEnabledForReferral.ToBool()))
                .ForMember(m => m.VenueIsRemoved, o => o.MapFrom(s => s.VenueIsRemoved.ToBool()))
                .ForMember(m => m.QualificationIsOffered, o => o.MapFrom(s => s.QualificationIsOffered.ToBool()))
                .ForMember(m => m.Routes, o => 
                    o.MapFrom(s => 
                        s.Routes.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(r => r.Trim())
                            .ToList()))
                ;
        }
    }
}