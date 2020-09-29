using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap
{
    public class When_ProvisionGapRepository_Update_Is_Called
    {
        private readonly Domain.Models.ProvisionGap _result;

        public When_ProvisionGapRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProvisionGap>>>();

            using var dbContext = InMemoryDbContext.Create();
            var entity = new ValidProvisionGapBuilder().Build();
            dbContext.Add(entity);
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.ProvisionGap>(logger, dbContext);

            entity.NoSuitableStudent = false;
            entity.HadBadExperience = true;
            entity.ProvidersTooFarAway = false;

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.NoSuitableStudent.Should().BeFalse();
            _result.HadBadExperience.Should().BeTrue();
            _result.ProvidersTooFarAway.Should().BeFalse();

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
