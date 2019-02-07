using AutoMapper;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Mappers;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_CreateEmployerDto_Is_Mapped_To_Employer
    {
        private MapperConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _config = new MapperConfiguration(c => c.AddProfile<EmployerMapper>());
        }

        [Test]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}