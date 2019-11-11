using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification
{
    public class When_ProviderQualificationRepository_Update_Is_Called
    {
        private readonly Domain.Models.ProviderQualification _result;

        public When_ProviderQualificationRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderQualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidProviderQualificationBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProviderQualification>(logger, dbContext);

                entity.ProviderVenueId = 2;
                entity.QualificationId = 3;
                entity.NumberOfPlacements = 4;
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
            _result.ProviderVenueId.Should().Be(2);
            _result.QualificationId.Should().Be(3);
            _result.NumberOfPlacements.Should().Be(4);
            _result.Source.Should().BeEquivalentTo("Updated");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
