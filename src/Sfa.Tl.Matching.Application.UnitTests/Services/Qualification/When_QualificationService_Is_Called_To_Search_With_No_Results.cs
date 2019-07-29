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
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    [Trait("QualificationService", "Search With No Results")]
    public class When_QualificationService_Is_Called_To_Search_With_No_Results
    {
        private readonly QualificationSearchViewModel _searchResult;

        public When_QualificationService_Is_Called_To_Search_With_No_Results()
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

                _searchResult = service.SearchQualificationAsync("No Qualification Exists").GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Search_Results_Should_Be_Returned()
        {
            _searchResult.SearchTerms.Should().Be("No Qualification Exists");
            _searchResult.Results.Count.Should().Be(0);
            _searchResult.ResultCount.Should().Be(0);
        }
    }
}