using System.Collections.Generic;
using System.Linq;
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
    public class When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called
    {
        private readonly IList<EmployerFeedbackDto> _result;

        public When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new ValidOpportunityBuilder()
                        .AddEmployer()
                        .AddReferrals(true)
                        .Build());
                dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, dbContext);
                var referralDate = EntityCreationConstants.ModifiedOn.AddDays(1);
                _result = repository.GetReferralsForEmployerFeedbackAsync(referralDate)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_A_Valid_Item_Is_Returned() =>
            _result.Should().NotBeNullOrEmpty();

        [Fact]
        public void Then_EmployerFeedbackDto_Fields_Are_As_Expected()
        {
            _result.First().OpportunityId.Should().Be(1);
            _result.First().OpportunityItemId.Should().Be(1);
            _result.First().EmployerContact.Should().BeEquivalentTo("Employer Contact");
            _result.First().EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
        }
    }
}