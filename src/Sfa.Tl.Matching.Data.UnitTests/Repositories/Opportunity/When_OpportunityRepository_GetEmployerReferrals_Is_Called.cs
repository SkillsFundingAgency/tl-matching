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
    public class When_OpportunityRepository_GetEmployerReferrals_Is_Called
    {
        private readonly EmployerReferralDto _result;

        public When_OpportunityRepository_GetEmployerReferrals_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new ValidOpportunityBuilder()
                        .AddEmployer()
                        .AddReferrals()
                        .AddProvisionGaps()
                        .Build());
                dbContext.SaveChanges();


                var repository = new OpportunityRepository(logger, dbContext);
                _result = repository.GetEmployerReferralsAsync(1, new[] { 1 })
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_EmployerReferralDto_Fields_Are_As_Expected()
        {
            _result.OpportunityId.Should().Be(1);
            _result.CompanyName.Should().BeEquivalentTo("Company");
            _result.PrimaryContact.Should().BeEquivalentTo("Employer Contact");
            _result.Phone.Should().BeEquivalentTo("020 123 4567");
            _result.Email.Should().BeEquivalentTo("employer.contact@employer.co.uk");
            _result.Postcode.Should().BeEquivalentTo("CV1 2WT");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
        }
    }
}