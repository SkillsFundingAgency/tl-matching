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
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<CheckAnswersPlacementViewModel, CheckAnswersDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<CheckAnswersPlacementViewModel, CheckAnswersDto>>())
                ;

            // Provision Gap
            CreateMap<CheckAnswersDto, CheckAnswersProvisionGapViewModel>()
                .ForMember(m => m.PlacementInformation, opt => opt.MapFrom(s => s))
                ;

            CreateMap<CheckAnswersProvisionGapViewModel, CheckAnswersDto>()
                .ForMember(m => m.OpportunityId,
                    o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.ConfirmationSelected,
                    o => o.MapFrom(s => s.ConfirmationSelected))
                .ForAllOtherMembers(config => config.Ignore())
                ;

            // Referrals
            CreateMap<CheckAnswersDto, CheckAnswersReferralViewModel>()
                .ForMember(m => m.PlacementInformation, opt => opt.MapFrom(s => s))
                .ForMember(m => m.Providers, opt => opt.Ignore())
                ;

            CreateMap<CheckAnswersReferralViewModel, CheckAnswersDto>()
                .ForMember(m => m.OpportunityId,
                    o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.ConfirmationSelected,
                    o => o.MapFrom(s => s.ConfirmationSelected))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<CheckAnswersReferralViewModel, CheckAnswersDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<CheckAnswersReferralViewModel, CheckAnswersDto>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<ReferralDto, ReferralsViewModel>();
        }
    }
}