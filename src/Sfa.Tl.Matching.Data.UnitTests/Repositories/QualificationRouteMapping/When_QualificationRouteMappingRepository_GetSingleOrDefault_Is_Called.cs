using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.QualificationRouteMapping _result;

        public When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRouteMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationRouteMappingListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new QualificationRouteMappingRepository(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Qualification.LarId.Should().BeEquivalentTo("1234567X");
            _result.Qualification.Title.Should().BeEquivalentTo("Full Qualification Title");
            _result.Qualification.ShortTitle.Should().BeEquivalentTo("Short Title");
            _result.RouteId.Should().Be(2);
            _result.Source.Should().BeEquivalentTo("Test");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}