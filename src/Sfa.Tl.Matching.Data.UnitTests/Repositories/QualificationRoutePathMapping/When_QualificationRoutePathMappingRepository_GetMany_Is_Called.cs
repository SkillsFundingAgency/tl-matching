using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.QualificationRoutePathMapping> _result;

        public When_QualificationRoutePathMappingRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationRoutePathMappingListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new QualificationRoutePathMappingRepository(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_QualificationRoutePathMappings_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_RoutePathMapping_Id_Is_Returned() => 
            _result.First().Id.Should().Be(QualificationRoutePathMappingConstants.Id);

        [Fact]
        public void Then_RoutePathMapping_LarsId_Is_Returned() =>
            _result.First().Qualification.LarsId.Should().Be(QualificationRoutePathMappingConstants.LarsId);

        [Fact]
        public void Then_RoutePathMapping_Title_Is_Returned() =>
            _result.First().Qualification.Title.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.Title);

        [Fact]
        public void Then_RoutePathMapping_ShortTitle_Is_Returned()
            => _result.First().Qualification.ShortTitle.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.ShortTitle);

        [Fact]
        public void Then_RoutePathMapping_PathId_Is_Returned()
            => _result.First().RouteId.Should().Be(QualificationRoutePathMappingConstants.RouteId);

        [Fact]
        public void Then_RoutePathMapping_Source_Is_Returned() =>
            _result.First().Source.Should().BeEquivalentTo(QualificationRoutePathMappingConstants.Source);

        [Fact]
        public void Then_Route_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Route_CreatedOn_Is_Returned() =>
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Route_ModifiedBy_Is_Returned() =>
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);

        [Fact]
        public void Then_Route_ModifiedOn_Is_Returned() =>
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}