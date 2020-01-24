using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_GetMany_Is_Called : IClassFixture<PathTestFixture>
    {
        private readonly IEnumerable<Domain.Models.Path> _result;

        public When_PathRepository_GetMany_Is_Called(PathTestFixture testFixture)
        {
            _result = testFixture.Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().Name.Should().BeEquivalentTo("Path 1");
            _result.First().Keywords.Should().BeEquivalentTo("Keyword1");
            _result.First().Summary.Should().BeEquivalentTo("Path 1 summary");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}