using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class SaveProximityDataMapper : Profile
    {
        public SaveProximityDataMapper()
        {
            CreateMap<PostCodeLookupResultDto, SaveProximityData>()
                .ForMember(m => m.ProviderVenueId, o => o.Ignore());
        }
    }
}