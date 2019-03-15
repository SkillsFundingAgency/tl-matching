using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers;
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
                ;

            CreateMap<EmployerDetailsViewModel, EmployerDetailDto>()
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>>())
                ;

            CreateMap<EmployerDto, EmployerDetailsViewModel>()
                .ForMember(m => m.Contact, o => o.MapFrom(s => s.PrimaryContact))
                .ForMember(m => m.ContactEmail, o => o.MapFrom(s => s.Email))
                .ForMember(m => m.ContactPhone, o => o.MapFrom(s => s.Phone))
                ;
        }
    }
}
