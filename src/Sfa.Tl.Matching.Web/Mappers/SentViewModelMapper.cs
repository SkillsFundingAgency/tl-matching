using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class SentViewModelMapper : Profile
    {
        public SentViewModelMapper()
        {
            CreateMap<OpportunityDto, ProvisionGapSentViewModel>()
                .ForMember(m => m.WithResults,
                    opt => opt.MapFrom(src => src.SearchResultProviderCount > 0))
                .ForMember(m => m.NoResults,
                    opt => opt.MapFrom(src => src.SearchResultProviderCount == 0))
                .ForMember(m => m.EmployerCrmRecord, opt => opt.Ignore())
                ;

            CreateMap<OpportunityDto, EmailsSentViewModel>()
                .ForMember(m => m.EmployerCrmRecord, opt => opt.Ignore())
                ;
        }
    }
}