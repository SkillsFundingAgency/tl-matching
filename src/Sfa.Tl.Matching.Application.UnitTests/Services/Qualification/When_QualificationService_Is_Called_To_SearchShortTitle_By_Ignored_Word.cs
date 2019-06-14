using System;
using System.Collections.Generic;
using System.Linq;
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
    [Trait("QualificationService", "Search Short Title By Ignored Words")]
    public class When_QualificationService_Is_Called_To_SearchShortTitle_By_Ignored_Word
    {
        private readonly IRepository<Domain.Models.Qualification> _repository;
        private readonly List<QualificationShortTitleSearchResultViewModel> _searchResults;

        public When_QualificationService_Is_Called_To_SearchShortTitle_By_Ignored_Word()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);
            _repository = Substitute.For<IRepository<Domain.Models.Qualification>>();

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();
            var qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRoutePathMapping>>();

            var service = new QualificationService(mapper, _repository, qualificationRoutePathMappingRepository, learningAimReferenceRepository);

            _searchResults = service.SearchShortTitle("the").ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _searchResults.Count.Should().Be(0);
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