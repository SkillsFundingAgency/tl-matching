using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_AutoMapper_Profiles_Are_Configured
    {
        private readonly MapperConfiguration _config;
        
        public When_AutoMapper_Profiles_Are_Configured()
        {
            _config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
        }

        [Fact]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}