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

            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationListBuilder()
                    .Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Qualification>(logger, dbContext);

                var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();
                var qualificationRouteMappingRepository = Substitute.For<IRepository<QualificationRouteMapping>>();

                var service = new QualificationService(mapper, repository, qualificationRouteMappingRepository,
                    learningAimReferenceRepository);

                _searchResults = service
                    .SearchShortTitleAsync("sport")
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_The_Expected_Search_Results_Are_Returned()
        {
            _searchResults.Count.Should().Be(2);

            _searchResults[0].ShortTitle.Should().Be("sport and enterprise in the community");
            _searchResults[1].ShortTitle.Should().Be("sport and enterprise in the community");
        }
    }
}