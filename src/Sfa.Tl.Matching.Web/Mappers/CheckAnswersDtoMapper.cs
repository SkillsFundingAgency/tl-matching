using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CheckAnswersDtoMapper : Profile
    {
        public CheckAnswersDtoMapper()
        {
            CreateMap<CheckAnswersDto, CheckAnswersViewModel>()
                .ForMember(m => m.Providers, opt => opt.Ignore())
                .ForMember(m => m.CompanyNameAka, o => o.Ignore())
                .ForMember(m => m.Navigation, o => o.Ignore())
                ;

            CreateMap<CheckAnswersViewModel, CheckAnswersDto>()
                .ForMember(m => m.OpportunityItemId,
                    o => o.MapFrom(s => s.OpportunityItemId))
                .ForMember(m => m.Postcode,
                    o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.IsSaved,
                    o => o.MapFrom(s => true))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<CheckAnswersViewModel, CheckAnswersDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<CheckAnswersViewModel, CheckAnswersDto>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<ReferralDto, ReferralsViewModel>();
        }
    }
}