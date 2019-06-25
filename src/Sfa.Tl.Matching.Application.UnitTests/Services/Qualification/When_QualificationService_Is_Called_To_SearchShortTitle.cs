//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using AutoMapper;
//using FluentAssertions;
//using NSubstitute;
//using Sfa.Tl.Matching.Application.Mappers;
//using Sfa.Tl.Matching.Application.Services;
//using Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders;
//using Sfa.Tl.Matching.Data.Interfaces;
//using Sfa.Tl.Matching.Domain.Models;
//using Sfa.Tl.Matching.Models.ViewModel;
//using Xunit;

//namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
//{
//    [Trait("QualificationService", "Search Short Title")]
//    public class When_QualificationService_Is_Called_To_SearchShortTitle
//    {
//        private readonly IRepository<Domain.Models.Qualification> _repository;
//        private readonly IList<QualificationShortTitleSearchResultViewModel> _searchResults;

//        private readonly QualificationShortTitleSearchResultViewModel _firstQualification;
//        private readonly QualificationShortTitleSearchResultViewModel _secondQualification;

//        public When_QualificationService_Is_Called_To_SearchShortTitle()
//        {
//            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
//            var mapper = new Mapper(config);
//            _repository = Substitute.For<IRepository<Domain.Models.Qualification>>();
//            _repository.GetMany(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
//                .Returns(new QualificiationSearchResultsBuilder().Build().AsQueryable());

//            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();
//            var qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRouteMapping>>();

//            var service = new QualificationService(mapper, _repository, qualificationRoutePathMappingRepository, learningAimReferenceRepository);

//            const string shortTitle = "sport and enterprise in the community";

//            _searchResults = service.SearchShortTitle(shortTitle).GetAwaiter().GetResult();

//            _firstQualification = _searchResults[0];
//            _secondQualification = _searchResults[1];
//        }

//        [Fact]
//        public void Then_The_Expected_Search_Results_Are_Returned()
//        {
//            _searchResults.Count.Should().Be(2);
//            _firstQualification.ShortTitle.Should().Be("sport and enterprise in the community");
//            _secondQualification.ShortTitle.Should().Be("sport and enterprise in the community");
//        }

//        [Fact]
//        public void Then_QualificationRouteMappingRepository_GetMany_Is_Called_Exactly_Once()
//        {
//            _repository
//                .Received(1)
//                .GetMany(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>());
//        }
//    }
//}