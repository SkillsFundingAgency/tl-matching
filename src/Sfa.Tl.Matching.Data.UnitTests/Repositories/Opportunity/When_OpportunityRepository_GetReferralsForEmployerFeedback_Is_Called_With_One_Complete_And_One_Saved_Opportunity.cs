using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called_With_One_Complete_And_One_Saved_Opportunity : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly IList<EmployerFeedbackDto> _result;

        public When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called_With_One_Complete_And_One_Saved_Opportunity()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            _dbContext = InMemoryDbContext.Create();
            {
                _dbContext.Add(
                    new ValidOpportunityBuilder()
                        .AddEmployer()
                        .AddReferrals(true)
                        .AddSavedOpportunityItem()
                        .Build());
                _dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, _dbContext);
                var referralDate = EntityCreationConstants.ModifiedOn.AddDays(1);
                _result = repository.GetReferralsForEmployerFeedbackAsync(referralDate)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_EmployerFeedbackDto_Items_Are_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}