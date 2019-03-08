using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class PlacementInformationMapper : Profile
    {
        public PlacementInformationMapper()
        {
            CreateMap<PlacementInformationViewModel, Opportunity>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.PostCode, config => config.Ignore())
                .ForMember(m => m.RouteId, config => config.Ignore())
                .ForMember(m => m.Distance, config => config.Ignore())
                .ForMember(m => m.EmployerName, config => config.Ignore())
                .ForMember(m => m.EmployerContact, config => config.Ignore())
                .ForMember(m => m.EmployerContactEmail, config => config.Ignore())
                .ForMember(m => m.EmployerContactPhone, config => config.Ignore())
                .ForMember(m => m.UserEmail, config => config.Ignore())
                .ForMember(m => m.IsReferral, config => config.Ignore())
                .ForMember(m => m.DropOffStage, config => config.Ignore())
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.ProvisionGap, config => config.Ignore())
                .ForMember(m => m.Referral, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom
                        (src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ? src.Placements : 1));
        }
    }
}