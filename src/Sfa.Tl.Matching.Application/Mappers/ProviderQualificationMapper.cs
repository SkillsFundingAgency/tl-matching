using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderQualificationMapper : Profile
    {
        public ProviderQualificationMapper()
        {
            CreateMap<ProviderQualificationDto, ProviderQualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.Qualification, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());

            CreateMap<AddQualificationViewModel, ProviderQualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.NumberOfPlacements, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.Qualification, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<AddQualificationViewModel, ProviderQualification>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());
        }
    }
}