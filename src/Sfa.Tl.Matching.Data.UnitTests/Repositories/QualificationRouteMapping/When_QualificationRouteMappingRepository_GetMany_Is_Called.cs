using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.QualificationRouteMapping> _result;

        public When_QualificationRouteMappingRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRouteMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationRouteMappingListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new QualificationRouteMappingRepository(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_The_QualificationRouteMappings_Is_Returned_AS_Expected()
        {
            _result.Count().Should().Be(2);
            _result.First().Id.Should().Be(QualificationRouteMappingConstants.Id);
            _result.First().Qualification.LarsId.Should().Be(QualificationRouteMappingConstants.LarsId);
            _result.First().Qualification.Title.Should().BeEquivalentTo(QualificationRouteMappingConstants.Title);
            _result.First().Qualification.ShortTitle.Should().BeEquivalentTo(QualificationRouteMappingConstants.ShortTitle);
            _result.First().RouteId.Should().Be(QualificationRouteMappingConstants.RouteId);
            _result.First().Source.Should().BeEquivalentTo(QualificationRouteMappingConstants.Source);
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}