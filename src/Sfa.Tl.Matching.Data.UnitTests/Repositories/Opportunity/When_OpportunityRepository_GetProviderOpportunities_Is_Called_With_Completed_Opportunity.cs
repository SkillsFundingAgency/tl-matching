using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetProviderOpportunities_Is_Called_With_Completed_Opportunity
    {
        private readonly IList<OpportunityReferralDto> _result;

        public When_OpportunityRepository_GetProviderOpportunities_Is_Called_With_Completed_Opportunity()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            var dbContext = InMemoryDbContext.Create();

            var opportunity = new ValidOpportunityBuilder()
                .AddEmployer()
                .AddReferrals(true) //Completed
                .Build();
            
            var opportunityItem = opportunity.OpportunityItem.First();
            opportunityItem.IsSelectedForReferral = true;

            dbContext.Add(opportunity);
            dbContext.SaveChanges();

            var opportunityItemId = opportunity.OpportunityItem.First().Id;
            
            var repository = new OpportunityRepository(logger, dbContext);
            _result = repository.GetIncompleteProviderOpportunitiesAsync(
                    1, 
                    new List<int> { opportunityItemId })
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Items_Are_Returned() =>
            _result.Should().BeEmpty();
    }
}