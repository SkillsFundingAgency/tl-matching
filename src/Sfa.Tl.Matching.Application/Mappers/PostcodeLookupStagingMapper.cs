using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class PostcodeLookupStagingMapper : Profile
    {
        public PostcodeLookupStagingMapper()
        {
            CreateMap<PostcodeLookupStagingDto, PostcodeLookupStaging>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}