using AutoMapper;
using Newtonsoft.Json.Linq;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerStagingMapper : Profile
    {
        public EmployerStagingMapper()
        {
            CreateMap<EmployerStagingDto, EmployerStaging>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<JObject, EmployerStagingFileImportDto>()
                .ForMember(m => m.CrmId, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'accountid')].value")))
                .ForMember(m => m.CompanyName, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'name')].value")))
                //.ForMember(m => m.AlsoKnownAs, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                //.ForMember(m => m.Aupa, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                //.ForMember(m => m.CompanyType, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                //.ForMember(m => m.PrimaryContact, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                //.ForMember(m => m.Phone, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                //.ForMember(m => m.Email, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                .ForMember(m => m.Postcode, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'address1_postalcode')].value")))
                .ForMember(m => m.Owner, config => config.MapFrom(s => s.SelectToken("$.InputParameters[0].value.Attributes[?(@.key == 'ownerid')].value.LogicalName")))
                ;

            CreateMap<EmployerStagingFileImportDto, Employer>()
                .ForMember(m => m.CrmId, config => config.MapFrom(s => s.CrmId.ToGuid()))
                .ForMember(m => m.CompanyName, config => config.MapFrom(s => s.CompanyName))
                .ForMember(m => m.AlsoKnownAs, config => config.MapFrom(s => s.AlsoKnownAs))
                .ForMember(m => m.Aupa, config => config.MapFrom(s => s.Aupa))
                .ForMember(m => m.CompanyType, config => config.MapFrom(s => s.CompanyType))
                .ForMember(m => m.PrimaryContact, config => config.MapFrom(s => s.PrimaryContact))
                .ForMember(m => m.Phone, config => config.MapFrom(s => s.Phone))
                .ForMember(m => m.Email, config => config.MapFrom(s => s.Email))
                .ForMember(m => m.Postcode, config => config.MapFrom(s => s.Postcode))
                .ForMember(m => m.Owner, config => config.MapFrom(s => s.Owner))
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}