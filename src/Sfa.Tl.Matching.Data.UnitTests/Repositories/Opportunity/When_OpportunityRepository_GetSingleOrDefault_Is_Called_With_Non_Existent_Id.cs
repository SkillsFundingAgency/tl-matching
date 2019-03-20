using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.Opportunity _result;

        public When_OpportunityRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Opportunity>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidOpportunityBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Opportunity>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Opportunity_Is_Returned() =>
            _result.Should().BeNull();
    }
}