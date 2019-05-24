using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class LearningAimsReferenceStagingMapper : Profile
    {
        public LearningAimsReferenceStagingMapper()
        {
            CreateMap<LearningAimsReferenceStagingDto, LearningAimsReferenceStaging>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.LarId, config => config.Ignore())
                .ForMember(m => m.Title, config => config.Ignore())
                .ForMember(m => m.AwardOrgLarId, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}
