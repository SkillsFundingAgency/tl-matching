using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class BankHolidayMapper : Profile
    {
        public BankHolidayMapper()
        {
            CreateMap<BankHolidayResultDto, BankHoliday>()
                .ForMember(m => m.Date, config => config.MapFrom(s => s.Date))
                .ForMember(m => m.Title, config => config.MapFrom(s => s.Title))
                .ForMember(m => m.CreatedBy, config => config.MapFrom(s => "System"))
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                ;
        }
    }
}