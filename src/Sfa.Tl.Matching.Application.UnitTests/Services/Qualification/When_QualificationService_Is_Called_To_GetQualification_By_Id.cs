using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    public class When_QualificationService_Is_Called_To_GetQualification_By_Id
    {
        private readonly IRepository<Domain.Models.Qualification> _qualificationRepository;
        private readonly QualificationSearchResultViewModel _result;

        public When_QualificationService_Is_Called_To_GetQualification_By_Id()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();

            _qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();

            _qualificationRepository.GetSingleOrDefault(
                    Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Qualification, object>>[]>())
                .Returns(new ValidQualificationBuilder().BuildWithRoutes());

            var qualificationRouteMappingRepository = Substitute.For<IRepository<QualificationRouteMapping>>();

            var qualificationService = new QualificationService(mapper, _qualificationRepository, qualificationRouteMappingRepository, learningAimReferenceRepository);
            
            _result = qualificationService.GetQualificationByIdAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Qualification_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _qualificationRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Qualification, object>>[]>());
        }

        [Fact]
        public void Then_The_Qualification_Fields_Are_As_Expected()
        {
            _result.QualificationId.Should().Be(1);
            _result.LarId.Should().Be("10042982");
            _result.Title.Should().Be("Title");
            _result.ShortTitle.Should().Be("Short Title");
        }

        [Fact]
        public void Then_The_Qualification_Route_Fields_Are_As_Expected()
        {
            _result.RouteIds.Should().NotBeNull();
            _result.RouteIds.Count.Should().Be(2);
            _result.RouteIds.Should().Contain(1);
            _result.RouteIds.Should().Contain(2);
        }
    }
}