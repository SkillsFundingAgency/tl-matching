using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_CreateMany_Is_Called : IClassFixture<PathTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_PathRepository_CreateMany_Is_Called(PathTestFixture testFixture)
        {
            var paths = new List<Domain.Models.Path>
            {
                new Domain.Models.Path
                {
                    Id = 991,
                    Name = "Path 991",
                    CreatedBy =  EntityCreationConstants.CreatedByUser
                },
                new Domain.Models.Path
                {
                    Id = 992,
                    Name = "Path 992",
                    CreatedBy =  EntityCreationConstants.CreatedByUser
                }
            };

            testFixture.Builder.ClearData();

            _result = testFixture.Repository.CreateManyAsync(paths)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.Path.Count();

            foreach (var path in paths)
            {
                testFixture.Builder.Paths.Add(path);
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}