using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.OpportunityItem
{
    public class When_OpportunityItemRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.OpportunityItem _result;

        public When_OpportunityItemRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.OpportunityItem>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidOpportunityItemBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.OpportunityItem>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_OpportunityItem_Is_Returned() =>
            _result.Should().BeNull();
    }
}