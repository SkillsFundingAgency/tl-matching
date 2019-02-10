using AutoMapper;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Mappers;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_RoutePathMappingDto_Is_Mapped_To_RoutePathMapping
    {
        private MapperConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
        }

        [Test]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}