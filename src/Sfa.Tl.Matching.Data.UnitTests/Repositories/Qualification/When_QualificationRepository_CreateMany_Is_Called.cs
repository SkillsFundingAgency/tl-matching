using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_CreateMany_Is_Called : IClassFixture<QualificationTestFixture>
    {
        private readonly int _result;

        public When_QualificationRepository_CreateMany_Is_Called(QualificationTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreateQualification(1, "1000", "Title", "Short Title")
                .CreateQualification(2, "1001", "Title 2", "Short Title 2");

            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.Qualifications)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}