using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class EmployerDtoMapper : Profile
    {
        public EmployerDtoMapper()
        {
            CreateMap<FindEmployerViewModel, CompanyNameDto>()
                .ForMember(m => m.OpportunityItemId, opt => opt.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<FindEmployerViewModel, CompanyNameDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<FindEmployerViewModel, CompanyNameDto>>())
                .ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.SelectedEmployerCrmId))
                .ForMember(m => m.HasChanged, o => o.MapFrom(src => src.CompanyName != src.PreviousCompanyName))
                ;

            CreateMap<EmployerDetailsViewModel, EmployerDetailDto>()
                .ForMember(m => m.OpportunityItemId, opt => opt.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                ;

            CreateMap<OpportunityDto, EmployerDetailsViewModel>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.OpportunityItemId, o => o.Ignore())
                .ForMember(m => m.CompanyName, o => o.Ignore())
                .ForMember(m => m.AlsoKnownAs, o => o.Ignore())
                ;
        }
    }
}