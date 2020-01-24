using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called : IClassFixture<QualificationRouteMappingTestFixture>
    {
        private readonly Domain.Models.QualificationRouteMapping _result;

        public When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called(QualificationRouteMappingTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
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