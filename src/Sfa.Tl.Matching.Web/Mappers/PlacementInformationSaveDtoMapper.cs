using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class PlacementInformationSaveDtoMapper : Profile
    {
        public PlacementInformationSaveDtoMapper()
        {
            CreateMap<PlacementInformationSaveViewModel, PlacementInformationSaveDto>()
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ? 
                        src.Placements : 1))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom<LoggedInUserNameResolver<PlacementInformationSaveViewModel, PlacementInformationSaveDto>>())
                .ForMember(m => m.ModifiedOn, o => o.MapFrom<UtcNowResolver<PlacementInformationSaveViewModel, PlacementInformationSaveDto>>())
                ;

            CreateMap<PlacementInformationSaveDto, PlacementInformationSaveViewModel>()
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ? 
                        src.Placements : default))
                .ForMember(m => m.Navigation, o => o.Ignore())
                ;
        }
    }
}