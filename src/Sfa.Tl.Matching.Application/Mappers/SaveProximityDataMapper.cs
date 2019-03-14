using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class SaveProximityDataMapper : Profile
    {
        public SaveProximityDataMapper()
        {
            CreateMap<PostCodeLookupResultDto, SaveProximityData>()
                .ForMember(m => m.UkPrn, o => o.Ignore())
                .ForMember(m => m.PostCode, o => o.Ignore());
        }
    }
}