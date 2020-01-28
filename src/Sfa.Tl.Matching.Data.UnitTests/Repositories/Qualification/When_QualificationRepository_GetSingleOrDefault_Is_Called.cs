using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_GetSingleOrDefault_Is_Called : IClassFixture<QualificationTestFixture>
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_GetSingleOrDefault_Is_Called(QualificationTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.LarId.Should().BeEquivalentTo("1000X");
            _result.Title.Should().BeEquivalentTo("Title 1");
            _result.ShortTitle.Should().BeEquivalentTo("Short Title 1");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}