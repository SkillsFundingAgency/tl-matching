using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetMany_Is_Called : IClassFixture<RouteTestFixture>
    {
        private readonly IEnumerable<Domain.Models.Route> _result;

        public When_RouteRepository_GetMany_Is_Called(RouteTestFixture testFixture)
        {
            _result = testFixture.Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Routes_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().Name.Should().BeEquivalentTo("Route 1");
            _result.First().Keywords.Should().BeEquivalentTo("Keyword1");
            _result.First().Summary.Should().BeEquivalentTo("Route 1 summary");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}