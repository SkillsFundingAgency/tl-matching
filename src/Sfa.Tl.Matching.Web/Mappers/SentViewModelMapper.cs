using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class SentViewModelMapper : Profile
    {
        public SentViewModelMapper()
        {
            CreateMap<OpportunityDto, SentViewModel>()
                .ForMember(m=> m.PrimaryContact, config => config.MapFrom(s=> s.EmployerContact))
                .ForMember(m => m.EmployerCrmRecord, opt => opt.Ignore());
        }
    }
}