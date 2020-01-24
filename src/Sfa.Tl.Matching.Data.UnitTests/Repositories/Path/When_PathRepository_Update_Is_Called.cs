using System;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_Update_Is_Called : IClassFixture<PathTestFixture>
    {
        private readonly Domain.Models.Path _result;

        public When_PathRepository_Update_Is_Called(PathTestFixture testFixture)
        {
            var entity = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();

            entity.Name = "Updated Path Name";
            entity.Keywords = "Updated Keywords";
            entity.Summary = "Updated Summary";

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            testFixture.Repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Name.Should().BeEquivalentTo("Updated Path Name");
            _result.Keywords.Should().BeEquivalentTo("Updated Keywords");
            _result.Summary.Should().BeEquivalentTo("Updated Summary");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
