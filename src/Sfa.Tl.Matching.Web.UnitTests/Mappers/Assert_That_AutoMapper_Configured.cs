using AutoMapper;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers
{
    public class When_AutoMapper_Profiles_Are_Configured
    {
        private readonly MapperConfiguration _config;
        
        public When_AutoMapper_Profiles_Are_Configured()
        {
            _config = new MapperConfiguration(c => c.AddProfiles(typeof(SearchParametersViewModelMapper).Assembly));
        }

        [Fact]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}