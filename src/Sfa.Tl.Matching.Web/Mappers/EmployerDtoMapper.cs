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
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<FindEmployerViewModel, EmployerNameDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<FindEmployerViewModel, EmployerNameDto>>())
                .ForMember(m => m.EmployerId, o => o.MapFrom(s => s.SelectedEmployerId))
                .ForMember(m => m.EmployerCrmId, o => o.Ignore())
                ;

            CreateMap<EmployerDetailsViewModel, EmployerDetailDto>()
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                ;

            CreateMap<EmployerDto, EmployerDetailsViewModel>()
                .ForMember(m => m.OpportunityId, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.PrimaryContact))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.Email))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.Phone))
                ;

            CreateMap<OpportunityDto, EmployerDetailsViewModel>()
                .ForMember(m => m.CompanyName, 
                    o => o.MapFrom(s => s.EmployerName))
                .ForMember(m => m.OpportunityId,
                    o => o.MapFrom(s => s.Id))
                ;
        }
    }
}