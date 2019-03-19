using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetProviderOpportunities_Is_Called
    {
        private readonly IList<OpportunityReferralDto> _result;

        public When_OpportunityRepository_GetProviderOpportunities_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidOpportunityListBuilder().Build());
                //TODO: Add more data - Referral, Route, Provider,ProviderVenue
                dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, dbContext);
                _result = repository.GetProviderOpportunities(1)
                    .GetAwaiter().GetResult();
            }
        }

        //[Fact]
        //public void Then_One_Item_Is_Returned() =>
        //    _result.First().Count().Should().Be(1);

        //[Fact]
        //public void Then_OpportunityReferralDto_Id_Is_Returned() =>
        //    _result.First().First().OpportunityId.Should().Be(1);
        
        //[Fact]
        //public void Then_OpportunityReferralDto_RouteId_Is_Returned() =>
        //    _result.First().RouteId.Should().Be(2);

        //[Fact]
        //public void Then_OpportunityReferralDto_Postcode_Is_Returned() =>
        //    _result.First().Postcode.Should().BeEquivalentTo("AA1 1AA");

        //[Fact]
        //public void Then_OpportunityReferralDto_SearchRadius_Is_Returned()
        //    => _result.First().SearchRadius.Should().Be(10);

        //[Fact]
        //public void Then_OpportunityReferralDto_JobTitle_Is_Returned() =>
        //    _result.First().JobTitle.Should().BeEquivalentTo("Testing Job Title");

        //[Fact]
        //public void Then_OpportunityReferralDto_DropOffStage_Is_Returned()
        //    => _result.First().DropOffStage.Should().Be(9);

        //[Fact]
        //public void Then_OpportunityReferralDto_PlacementsKnown_Is_Returned()
        //    => _result.First().PlacementsKnown.Should().Be(true);

        //[Fact]
        //public void Then_OpportunityReferralDto_SearchResultProviderCount_Is_Returned()
        //    => _result.First().SearchResultProviderCount.Should().Be(12);

        //[Fact]
        //public void Then_OpportunityReferralDto_EmployerId_Is_Returned()
        //    => _result.First().EmployerId.Should().Be(5);

        //[Fact]
        //public void Then_OpportunityReferralDto_EmployerName_Is_Returned()
        //    => _result.First().EmployerName.Should().BeEquivalentTo("Employer");
        
        //[Fact]
        //public void Then_OpportunityReferralDto_EmployerContact_Is_Returned()
        //    => _result.First().EmployerContact.Should().BeEquivalentTo("Employer Contact");

        //[Fact]
        //public void Then_OpportunityReferralDto_EmployerContactPhone_Is_Returned()
        //    => _result.First().EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");

        //[Fact]
        //public void Then_OpportunityReferralDto_EmployerContactEmail_Is_Returned()
        //    => _result.First().EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        //[Fact]
        //public void Then_OpportunityReferralDto_UserEmail_Is_Returned()
        //    => _result.First().UserEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        //[Fact]
        //public void Then_OpportunityReferralDto_ConfirmationSelected_Is_Returned()
        //    => _result.First().ConfirmationSelected.Should().Be(true);
                
        //[Fact]
        //public void Then_OpportunityReferralDto_CreatedBy_Is_Returned() =>
        //    _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
    }
}