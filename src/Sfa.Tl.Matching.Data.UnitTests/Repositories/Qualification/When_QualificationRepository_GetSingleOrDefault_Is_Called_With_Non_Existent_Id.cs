using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id : IClassFixture<QualificationTestFixture>
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id(QualificationTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 99)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Qualification_Is_Returned() =>
            _result.Should().BeNull();
    }
}