using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    public class When_QualificationService_Is_Called_To_Check_IsValidLarId_With_Valid_Lar_Id
    {
        private readonly bool _result;

        public When_QualificationService_Is_Called_To_Check_IsValidLarId_With_Valid_Lar_Id()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();

            var qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();
            var qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRoutePathMapping>>();

            var qualificationService = new QualificationService(mapper, qualificationRepository, qualificationRoutePathMappingRepository, learningAimReferenceRepository);

            _result = qualificationService.IsValidLarIdAsync("12345678").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _result.Should().BeTrue();
        }
    }
}