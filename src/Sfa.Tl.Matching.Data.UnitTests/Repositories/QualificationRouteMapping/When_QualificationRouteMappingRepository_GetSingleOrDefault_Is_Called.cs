using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Constants;
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
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned()
        {
            _result.Id.Should().Be(QualificationRouteMappingConstants.Id);
            _result.Qualification.LarsId.Should().BeEquivalentTo(QualificationRouteMappingConstants.LarsId);
            _result.Qualification.Title.Should().BeEquivalentTo(QualificationRouteMappingConstants.Title);
            _result.Qualification.ShortTitle.Should().BeEquivalentTo(QualificationRouteMappingConstants.ShortTitle);
            _result.RouteId.Should().Be(QualificationRouteMappingConstants.RouteId);
            _result.Source.Should().BeEquivalentTo(QualificationRouteMappingConstants.Source);
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}