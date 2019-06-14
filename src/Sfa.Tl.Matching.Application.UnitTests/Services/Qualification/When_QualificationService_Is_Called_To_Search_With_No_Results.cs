using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    [Trait("QualificationService", "Search With No Results")]
    public class When_QualificationService_Is_Called_To_Search_With_No_Results
    {
        private readonly IRepository<Domain.Models.Qualification> _repository;
        private readonly QualificationSearchViewModel _searchResult;

        public When_QualificationService_Is_Called_To_Search_With_No_Results()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);
            _repository = Substitute.For<IRepository<Domain.Models.Qualification>>();
            _repository.Count(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(0);

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();
            var qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRoutePathMapping>>();

            var service = new QualificationService(mapper, _repository, qualificationRoutePathMappingRepository, learningAimReferenceRepository);

            _searchResult = service.SearchQualificationAsync("No Qualification Exists").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Search_Results_Should_Be_Returned()
        {
            _searchResult.SearchTerms.Should().Be("No Qualification Exists");
            _searchResult.Results.Count.Should().Be(0);
            _searchResult.ResultCount.Should().Be(0);
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_Count_Is_Called_Exatly_Once()
        {
            _repository
                .Received(1)
                .Count(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>());
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_GetMany_Is_Not_Called()
        {
            _repository
                .DidNotReceive()
                .GetMany(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>());
        }
    }
}