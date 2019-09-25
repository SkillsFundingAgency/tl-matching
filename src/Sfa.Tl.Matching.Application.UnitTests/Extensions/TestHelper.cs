using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    public static class TestHelper
    {
        public static string GetTestExecutionDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }

        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            return new Mapper(config);
        }

        public static IDataParser<TDto> GetDataParser<TDto>() where TDto : class
        {
            return (IDataParser<TDto>)Activator.CreateInstance(typeof(EmployerStagingDataParser).Assembly.GetTypes()
                .First(t => typeof(IDataParser<TDto>).IsAssignableFrom(t)));
        }
    }
}