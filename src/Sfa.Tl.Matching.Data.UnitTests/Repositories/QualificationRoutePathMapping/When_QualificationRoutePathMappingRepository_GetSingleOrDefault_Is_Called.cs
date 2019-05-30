using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.QualificationRoutePathMapping _result;

        public When_QualificationRoutePathMappingRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationRoutePathMappingListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new QualificationRoutePathMappingRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned() =>
            _result.Id.Should().Be(QualificationRoutePathMappingConstants.Id);

        [Fact]
        public void Then_RoutePathMapping_LarsId_Is_Returned() =>
            _result.Qualification.LarsId.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.LarsId);

        [Fact]
        public void Then_RoutePathMapping_Title_Is_Returned() =>
            _result.Qualification.Title.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.Title);

        [Fact]
        public void Then_RoutePathMapping_ShortTitle_Is_Returned() => 
            _result.Qualification.ShortTitle.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.ShortTitle);

        [Fact]
        public void Then_RoutePathMapping_RouteId_Is_Returned()
            => _result.RouteId.Should().Be(QualificationRoutePathMappingConstants.RouteId);

        [Fact]
        public void Then_RoutePathMapping_Source_Is_Returned() =>
            _result.Source.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.Source);
        
        [Fact]
        public void Then_RoutePathMapping_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_RoutePathMapping_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_RoutePathMapping_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_RoutePathMapping_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}