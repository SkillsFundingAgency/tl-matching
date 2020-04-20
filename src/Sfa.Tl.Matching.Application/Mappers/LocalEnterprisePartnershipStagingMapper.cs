using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class LocalEnterprisePartnershipStagingMapper : Profile
    {
        public LocalEnterprisePartnershipStagingMapper()
        {
            CreateMap<LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStaging>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}