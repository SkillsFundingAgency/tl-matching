using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_CreateMany_Is_Called : IClassFixture<QualificationRouteMappingTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_QualificationRouteMappingRepository_CreateMany_Is_Called(QualificationRouteMappingTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreateQualificationRouteMapping(3, 1)
                .CreateQualificationRouteMapping(4, 2);
            
            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.QualificationRouteMappings)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.QualificationRouteMapping.Count();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            //This is returning four because qualification objects are also getting inserted
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}