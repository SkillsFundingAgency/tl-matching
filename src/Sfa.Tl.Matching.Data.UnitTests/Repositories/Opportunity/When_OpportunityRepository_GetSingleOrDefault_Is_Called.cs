using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Opportunity _result;

        public When_OpportunityRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Opportunity>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidOpportunityListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Opportunity>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Opportunity_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_Opportunity_RouteId_Is_Returned() =>
            _result.RouteId.Should().Be(1);

        [Fact]
        public void Then_Opportunity_Postcode_Is_Returned() =>
            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_Opportunity_SearchRadius_Is_Returned()
            => _result.SearchRadius.Should().Be(10);

        [Fact]
        public void Then_Opportunity_JobTitle_Is_Returned() =>
            _result.JobTitle.Should().BeEquivalentTo("Testing Job Title");

        [Fact]
        public void Then_Opportunity_DropOffStage_Is_Returned()
            => _result.DropOffStage.Should().Be(9);

        [Fact]
        public void Then_Opportunity_PlacementsKnown_Is_Returned()
            => _result.PlacementsKnown.Should().Be(true);

        [Fact]
        public void Then_Opportunity_SearchResultProviderCount_Is_Returned()
            => _result.SearchResultProviderCount.Should().Be(12);

        [Fact]
        public void Then_Opportunity_EmployerId_Is_Returned()
            => _result.EmployerId.Should().Be(5);

        [Fact]
        public void Then_Opportunity_EmployerName_Is_Returned()
            => _result.EmployerName.Should().BeEquivalentTo("Employer");
        
        [Fact]
        public void Then_Opportunity_EmployerContact_Is_Returned()
            => _result.EmployerContact.Should().BeEquivalentTo("Employer Contact");

        [Fact]
        public void Then_Opportunity_EmployerContactPhone_Is_Returned()
            => _result.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");

        [Fact]
        public void Then_Opportunity_EmployerContactEmail_Is_Returned()
            => _result.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        [Fact]
        public void Then_Opportunity_UserEmail_Is_Returned()
            => _result.UserEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        [Fact]
        public void Then_Opportunity_ConfirmationSelected_Is_Returned()
            => _result.ConfirmationSelected.Should().Be(true);
                
        [Fact]
        public void Then_Opportunity_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Opportunity_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Opportunity_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_Opportunity_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}