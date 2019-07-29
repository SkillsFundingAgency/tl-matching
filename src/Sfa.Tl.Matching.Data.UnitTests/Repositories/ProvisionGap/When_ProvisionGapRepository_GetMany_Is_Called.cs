using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProvisionGap
{
    public class When_ProvisionGapRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.ProvisionGap> _result;

        public When_ProvisionGapRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProvisionGap>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProvisionGapListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProvisionGap>(logger, dbContext);

                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_ProvisionGaps_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Item_Fields_Are_As_Expected()
        {
            var provisionGap = _result.First();
            provisionGap.Should().NotBeNull();

            provisionGap.Id.Should().Be(1);
            provisionGap.NoSuitableStudent.Should().BeTrue();
            provisionGap.HadBadExperience.Should().BeFalse();
            provisionGap.ProvidersTooFarAway.Should().BeTrue();
            provisionGap.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            provisionGap.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            provisionGap.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            provisionGap.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
        
        [Fact]
        public void Then_Second_Item_Fields_Are_As_Expected()
        {
            var provisionGap = _result.Skip(1).First();
            provisionGap.Should().NotBeNull();

            provisionGap.Id.Should().Be(2);
            provisionGap.NoSuitableStudent.Should().BeFalse();
            provisionGap.HadBadExperience.Should().BeTrue();
            provisionGap.ProvidersTooFarAway.Should().BeFalse();
            provisionGap.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            provisionGap.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            provisionGap.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            provisionGap.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);

        }
    }
}