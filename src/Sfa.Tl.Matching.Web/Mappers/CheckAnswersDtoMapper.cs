using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CheckAnswersDtoMapper : Profile
    {
        public CheckAnswersDtoMapper()
        {
            CreateMap<CheckAnswersPlacementViewModel, CheckAnswersDto>()
                .ForMember(m => m.ConfirmationSelected, opt => opt.Ignore())
                .ForMember(m => m.OpportunityId, opt => opt.Ignore())
                .ForMember(m => m.OpportunityItemId, opt => opt.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                ;

            // Referrals
            CreateMap<CheckAnswersDto, CheckAnswersViewModel>()
                .ForMember(m => m.PlacementInformation, opt => opt.MapFrom(s => s))
                .ForMember(m => m.Providers, opt => opt.Ignore())
                ;

            CreateMap<CheckAnswersViewModel, CheckAnswersDto>()
                .ForMember(m => m.OpportunityItemId,
                    o => o.MapFrom(s => s.OpportunityItemId))
                .ForMember(m => m.Postcode,
                    o => o.MapFrom(s => s.Postcode))

                .ForMember(m => m.ConfirmationSelected,
                    o => o.MapFrom(s => true))

                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<CheckAnswersViewModel, CheckAnswersDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<CheckAnswersViewModel, CheckAnswersDto>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<ReferralDto, ReferralsViewModel>();
        }
    }
}