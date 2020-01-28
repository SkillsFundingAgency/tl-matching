using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_GetMany_Is_Called : IClassFixture<QualificationRouteMappingTestFixture>
    {
        private readonly IEnumerable<Domain.Models.QualificationRouteMapping> _result;

        public When_QualificationRouteMappingRepository_GetMany_Is_Called(QualificationRouteMappingTestFixture testFixture)
        {
            _result = testFixture.Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Count().Should().Be(2);
            _result.First().Id.Should().Be(1);
            _result.First().Qualification.LarId.Should().Be("1000X");
            _result.First().Qualification.Title.Should().BeEquivalentTo("Qualification Title 1");
            _result.First().Qualification.ShortTitle.Should().BeEquivalentTo("Short Title 1");
            _result.First().RouteId.Should().Be(2);
            _result.First().Source.Should().BeEquivalentTo("Test");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}