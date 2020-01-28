using System;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_Update_Is_Called : IClassFixture<QualificationTestFixture>
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_Update_Is_Called(QualificationTestFixture testFixture)
        {
            var entity = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();

            entity.Title = "Updated Title";
            entity.ShortTitle = "Updated Short Title";

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
            _result.LarId.Should().BeEquivalentTo("1000X");
            _result.Title.Should().BeEquivalentTo("Updated Title");
            _result.ShortTitle.Should().BeEquivalentTo("Updated Short Title");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
