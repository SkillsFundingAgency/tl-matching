using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_Update_Is_Called
    {
        private readonly Domain.Models.QualificationRouteMapping _result;

        public When_QualificationRouteMappingRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.QualificationRouteMapping>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidQualificationRouteMappingBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.QualificationRouteMapping>(logger, dbContext);

                entity.Qualification.LarsId = "1234567X";
                entity.Qualification.Title = "Updated Full Qualification Title";
                entity.Qualification.ShortTitle = "Updated Short Title";
                entity.RouteId = 5;
                entity.Source = "Updated";

                entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
                entity.ModifiedBy = "UpdateTestUser";

                repository.UpdateAsync(entity).GetAwaiter().GetResult();

                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Qualification.LarsId.Should().BeEquivalentTo("1234567X");
            _result.Qualification.Title.Should().BeEquivalentTo("Updated Full Qualification Title");
            _result.Qualification.ShortTitle.Should().BeEquivalentTo("Updated Short Title");
            _result.RouteId.Should().Be(5);
            _result.Source.Should().BeEquivalentTo("Updated");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
