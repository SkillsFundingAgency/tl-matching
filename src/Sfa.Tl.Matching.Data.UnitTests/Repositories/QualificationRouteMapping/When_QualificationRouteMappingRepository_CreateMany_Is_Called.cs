using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_CreateMany_Is_Called : IClassFixture<QualificationRouteMappingTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_QualificationRouteMappingRepository_CreateMany_Is_Called(QualificationRouteMappingTestFixture testFixture)
        {
            var qualificationRouteMappings = new List<Domain.Models.QualificationRouteMapping>
            {
                new Domain.Models.QualificationRouteMapping
                {
                    Id = 991,
                    RouteId = 2,
                    Source = "Test",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    Qualification = new Domain.Models.Qualification
                    {
                        LarId = "13579XXX",
                        Title = "New Title",
                        ShortTitle = "Short Title"
                    }
                },
                new Domain.Models.QualificationRouteMapping
                {
                    Id = 992,
                    RouteId = 3,
                    Source = "Test",
                    CreatedBy =  EntityCreationConstants.CreatedByUser,
                    Qualification = new Domain.Models.Qualification
                    {
                        LarId = "24680XXX",
                        Title = "Another New Title",
                        ShortTitle = "Another Short Title"
                    }
                }
            };

            testFixture.Builder.ClearData();

            _result = testFixture.Repository.CreateManyAsync(qualificationRouteMappings)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.QualificationRouteMapping.Count();

            foreach (var qrpm in qualificationRouteMappings)
            {
                testFixture.Builder.QualificationRouteMappings.Add(qrpm);
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            //This is returning four because qualification objects are also getting inserted
            _result.Should().Be(4);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}