using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class EmployerDtoMapper : Profile
    {
        public EmployerDtoMapper()
        {
            CreateMap<FindEmployerViewModel, EmployerNameDto>()
                .ForMember(m => m.OpportunityItemId, opt => opt.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<FindEmployerViewModel, EmployerNameDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<FindEmployerViewModel, EmployerNameDto>>())
                .ForMember(m => m.EmployerId, o => o.MapFrom(s => s.SelectedEmployerId))
                .ForMember(m => m.EmployerCrmId, o => o.Ignore())
                .ForMember(m => m.HasChanged, o => o.MapFrom(src => src.CompanyName != src.PreviousCompanyName))
                ;

            CreateMap<EmployerDetailsViewModel, EmployerDetailDto>()
                .ForMember(m => m.OpportunityItemId, opt => opt.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                ;

            CreateMap<EmployerStagingDto, EmployerDetailsViewModel>()
                .ForMember(m => m.OpportunityId, o => o.Ignore())
                .ForMember(m => m.EmployerName, o => o.MapFrom(s => s.CompanyName))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.PrimaryContact))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.Email))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.Phone))
                ;

            CreateMap<OpportunityDto, EmployerDetailsViewModel>()
                .ForMember(m => m.OpportunityId,
                    o => o.MapFrom(s => s.Id))
                .ForMember(m => m.EmployerName, o => o.Ignore())
                ;
        }
    }
}