using System;
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
    public class When_OpportunityItemRepository_Update_Is_Called
    {
        private readonly Domain.Models.OpportunityItem _result;

        public When_OpportunityItemRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.OpportunityItem>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidOpportunityItemBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.OpportunityItem>(logger, dbContext);

                entity.RouteId = 2;
                entity.OpportunityType = OpportunityType.ProvisionGap.ToString();
                entity.Postcode = "BB1 1BB";
                entity.JobRole = "Updated Job Title";
                entity.PlacementsKnown = false;
                entity.Placements = 9;
                entity.SearchResultProviderCount = 25;
                entity.IsSaved = false;
                entity.IsSelectedForReferral = false;
                entity.IsCompleted = false;

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
            _result.RouteId.Should().Be(2);
            _result.OpportunityType.Should().Be(OpportunityType.ProvisionGap.ToString());
            _result.Postcode.Should().BeEquivalentTo("BB1 1BB");
            _result.JobRole.Should().BeEquivalentTo("Updated Job Title");
            _result.PlacementsKnown.Should().BeFalse();
            _result.Placements.Should().Be(9);
            _result.SearchResultProviderCount.Should().Be(25);
            _result.IsSaved.Should().BeFalse();
            _result.IsSelectedForReferral.Should().BeFalse();
            _result.IsCompleted.Should().BeFalse();

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
