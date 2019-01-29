using AutoMapper.Configuration;
using Sfa.Tl.Matching.FileReader.Excel.Employer;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class FileEmployerMapper : MapperConfigurationExpression
    {
        public FileEmployerMapper()
        {
            CreateMap<FileEmployer, CreateEmployerDto>();
        }
    }
}