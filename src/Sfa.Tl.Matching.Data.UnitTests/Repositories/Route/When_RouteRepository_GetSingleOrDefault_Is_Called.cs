using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called : IClassFixture<RouteTestFixture>
    {
        private readonly Domain.Models.Route _result;

        public When_RouteRepository_GetSingleOrDefault_Is_Called(RouteTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                   .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Name.Should().BeEquivalentTo("Route 1");
            _result.Keywords.Should().BeEquivalentTo("Keyword1");
            _result.Summary.Should().BeEquivalentTo("Route 1 summary");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}