using AutoMapper;

using Sfa.Tl.Matching.Application.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_RoutePathMappingDto_Is_Mapped_To_RoutePathMapping
    {
        private MapperConfiguration _config;

        
        public void Setup()
        {
            _config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
        }

        [Fact]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}