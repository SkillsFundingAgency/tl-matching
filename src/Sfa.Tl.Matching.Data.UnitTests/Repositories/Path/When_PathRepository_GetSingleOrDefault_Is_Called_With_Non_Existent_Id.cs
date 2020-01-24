using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id : IClassFixture<PathTestFixture>
    {
        private readonly Domain.Models.Path _result;

        public When_PathRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id(PathTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 99)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Path_Is_Returned() =>
            _result.Should().BeNull();
    }
}