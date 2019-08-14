using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem.Builders;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem
{
    public class When_OpportunityItemRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.OpportunityItem _result;

        public When_OpportunityItemRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.OpportunityItem>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidOpportunityItemListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.OpportunityItem>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.RouteId.Should().Be(1);
            _result.OpportunityType.Should().Be(OpportunityType.Referral.ToString());
            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");
            _result.SearchRadius.Should().Be(10);
            _result.JobRole.Should().BeEquivalentTo("Testing Job Title");
            _result.PlacementsKnown.Should().BeTrue();
            _result.SearchResultProviderCount.Should().Be(12);
            _result.IsSaved.Should().BeTrue();
            _result.IsSelectedForReferral.Should().BeTrue();
            _result.IsCompleted.Should().BeTrue();
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}