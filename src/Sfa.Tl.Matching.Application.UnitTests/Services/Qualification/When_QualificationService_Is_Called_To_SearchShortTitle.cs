using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    [Trait("QualificationService", "Search Short Title")]
    public class When_QualificationService_Is_Called_To_SearchShortTitle
    {
        private readonly IList<QualificationShortTitleSearchResultViewModel> _searchResults;

        public When_QualificationService_Is_Called_To_SearchShortTitle()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);

            var qlogger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();
            var qrmlogger = Substitute.For<ILogger<GenericRepository<QualificationRouteMapping>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidQualificationListBuilder().Build());
            dbContext.AddRange(
                new QualificationRouteMapping
                {
                    RouteId = 1,
                    QualificationId = 1
                }, 
                new QualificationRouteMapping
                {
                    RouteId = 1,
                    QualificationId = 2
                },
                new QualificationRouteMapping
                {
                    RouteId = 1,
                    QualificationId = 3
                });
            dbContext.SaveChanges();

            var qualificationRepo = new GenericRepository<Domain.Models.Qualification>(qlogger, dbContext);
            var routeMappingRepo = new GenericRepository<QualificationRouteMapping>(qrmlogger, dbContext);

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();

            var service = new QualificationService(mapper, qualificationRepo, routeMappingRepo, learningAimReferenceRepository);

            _searchResults = service.SearchShortTitle("sport");
        }

        [Fact]
        public void Then_The_Expected_Search_Results_Are_Returned()
        {
            _searchResults.Count.Should().Be(1);

            _searchResults[0].ShortTitle.Should().Be("sport and enterprise in the community");
        }
    }
}