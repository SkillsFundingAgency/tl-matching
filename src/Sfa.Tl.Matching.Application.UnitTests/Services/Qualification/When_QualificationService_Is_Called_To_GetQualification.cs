using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    public class When_QualificationService_Is_Called_To_GetQualification
    {
        private readonly IRepository<Domain.Models.Qualification> _qualificationRepository;
        private readonly QualificationDetailViewModel _result;

        public When_QualificationService_Is_Called_To_GetQualification()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);
            _qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();

            _qualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(new ValidQualificationBuilder().Build());

            var qualificationService = new QualificationService(mapper, _qualificationRepository);
            
            _result = qualificationService.GetQualificationAsync("10042982").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Qualification_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _qualificationRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>());
        }

        [Fact]
        public void Then_The_Qualification_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.LarsId.Should().Be("10042982");
            _result.Title.Should().Be("Title");
            _result.ShortTitle.Should().Be("Short Title");
        }
    }
}