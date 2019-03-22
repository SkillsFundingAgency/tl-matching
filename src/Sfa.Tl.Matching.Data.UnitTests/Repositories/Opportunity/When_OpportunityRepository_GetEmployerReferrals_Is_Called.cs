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
    public class When_OpportunityRepository_GetEmployerReferrals_Is_Called
    {
        private readonly EmployerReferralDto _result;

        public When_OpportunityRepository_GetEmployerReferrals_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidOpportunityReferralBuilder().Build());
                dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, dbContext);
                _result = repository.GetEmployerReferrals(1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_OpportunityId_Is_Returned() =>
            _result.OpportunityId.Should().Be(1);
        
        [Fact]
        public void Then_ReferralId_Is_Returned() =>
            _result.ProviderReferralInfo.First().ReferralId.Should().Be(1);
   
        [Fact]
        public void Then_EmployerName_Is_Returned()
            => _result.EmployerName.Should().BeEquivalentTo("Employer");

        [Fact]
        public void Then_EmployerContact_Is_Returned()
            => _result.EmployerContact.Should().BeEquivalentTo("Employer Contact");

        [Fact]
        public void Then_EmployerContactPhone_Is_Returned()
            => _result.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");

        [Fact]
        public void Then_EmployerContactEmail_Is_Returned()
            => _result.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        [Fact]
        public void Then_JobTitle_Is_Returned() =>
            _result.JobTitle.Should().BeEquivalentTo("Testing Job Title");

        [Fact]
        public void Then_Postcode_Is_Returned() =>
            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_PlacementsKnown_Is_Returned()
            => _result.PlacementsKnown.Should().Be(true);

        [Fact]
        public void Then_Placements_Is_Returned()
            => _result.Placements.Should().Be(3);

        [Fact]
        public void Then_RouteName_Is_Returned() =>
            _result.RouteName.Should().BeEquivalentTo("Test Route");

        [Fact]
        public void Then_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_One_ProviderReferralInfo_Item_Is_Returned() =>
            _result.ProviderReferralInfo.Count().Should().Be(1);
        
        [Fact]
        public void Then_ProviderName_Is_Returned() =>
            _result.ProviderReferralInfo.First().ProviderName.Should().BeEquivalentTo("ProviderName");

        [Fact]
        public void Then_ProviderPrimaryContact_Is_Returned() =>
            _result.ProviderReferralInfo.First().ProviderPrimaryContact.Should().BeEquivalentTo("PrimaryContact");

        [Fact]
        public void Then_ProviderPrimaryContactEmail_Is_Returned() =>
            _result.ProviderReferralInfo.First().ProviderPrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");
        
        [Fact]
        public void Then_ProviderPrimaryContactPhone_Is_Returned() =>
            _result.ProviderReferralInfo.First().ProviderPrimaryContactPhone.Should().BeEquivalentTo("01777757777");

        [Fact]
        public void Then_ProviderVenuePostcode_Is_Returned() =>
            _result.ProviderReferralInfo.First().ProviderVenuePostcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_One_QualificationShortTitles_Item_Is_Returned() =>
            _result.ProviderReferralInfo.First()
                .QualificationShortTitles
                .Count()
                .Should().Be(2);

        [Fact] public void Then_QualificationShortTitles_Includes_First_ShortTitle() =>
            _result.ProviderReferralInfo.First()
                .QualificationShortTitles
                .Should()
                .Contain("Short Title");

        [Fact]
        public void Then_QualificationShortTitles_Includes_Second_ShortTitle() =>
            _result.ProviderReferralInfo.First()
                .QualificationShortTitles
                .Should()
                .Contain("Duplicate Short Title");
    }
}