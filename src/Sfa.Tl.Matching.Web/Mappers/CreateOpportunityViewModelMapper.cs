using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CreateOpportunityViewModelMapper : Profile
    {
        public CreateOpportunityViewModelMapper()
        {
            CreateMap<CreateProvisionGapViewModel, OpportunityDto>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom<CreatedByUserNameResolver>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<CreatedByUserEmailResolver>())
                .ForMember(m => m.CreatedBy, o => o.Ignore())
                .ForMember(m => m.DropOffStage, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.Ignore())
                .ForMember(m => m.EmployerContactEmail, o => o.Ignore())
                .ForMember(m => m.EmployerContactPhone, o => o.Ignore())
                .ForMember(m => m.CreatedBy, o => o.Ignore())
                .ForMember(m => m.CreatedBy, o => o.Ignore())
                ;

            CreateMap<CreateReferralViewModel, OpportunityDto>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom<CreatedByUserNameResolver>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<CreatedByUserEmailResolver>())
                ;

            CreateMap<SelectedProviderViewModel, ReferralDto>();
        }
    }
}
