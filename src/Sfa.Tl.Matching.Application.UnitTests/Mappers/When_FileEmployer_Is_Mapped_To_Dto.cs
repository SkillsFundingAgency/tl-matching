using AutoMapper;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Mappers;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_FileEmployer_Is_Mapped_To_Dto
    {
        private Mapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingProfile = new FileEmployerMapper();

            var config = new MapperConfiguration(mappingProfile);
            _mapper = new Mapper(config);
        }

        [Test]
        public void Then_All_Properties_Are_Implemented()
        {
            var mapper = _mapper as IMapper;
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}