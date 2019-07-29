//using System.Collections.Generic;
//using System.Linq;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Sfa.Tl.Matching.Data.Repositories;
//using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
//using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
//using Sfa.Tl.Matching.Models.Dto;
//using Xunit;

//namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
//{
//    public class When_OpportunityRepository_GetProviderOpportunities_Is_Called
//    {
//        private readonly IList<OpportunityReferralDto> _result;

//        public When_OpportunityRepository_GetProviderOpportunities_Is_Called()
//        {
//            var logger = Substitute.For<ILogger<OpportunityRepository>>();

//            using (var dbContext = InMemoryDbContext.Create())
//            {
//                dbContext.Add(new ValidOpportunityReferralBuilder().Build());
//                dbContext.SaveChanges();

//                var repository = new OpportunityRepository(logger, dbContext);
//                _result = repository.GetProviderOpportunities(1)
//                    .GetAwaiter().GetResult();
//            }
//        }

//        [Fact]
//        public void Then_One_Item_Is_Returned() =>
//            _result.Count.Should().Be(1);

//        [Fact]
//        public void Then_OpportunityReferralDto_Fields_Are_As_Expected()
//        {
//            var dto = _result.First();
//            dto.OpportunityId.Should().Be(1);
//            dto.ReferralId.Should().Be(1);
//            dto.ProviderName.Should().BeEquivalentTo("ProviderName");
//            dto.ProviderPrimaryContact.Should().BeEquivalentTo("PrimaryContact");
//            dto.ProviderPrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");
//            dto.ProviderSecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");
//            dto.ProviderVenuePostcode.Should().BeEquivalentTo("AA1 1AA");
//            dto.RouteName.Should().BeEquivalentTo("Test Route");
//            dto.SearchRadius.Should().Be(10);
//            dto.Postcode.Should().BeEquivalentTo("AA1 1AA");
//            dto.JobRole.Should().BeEquivalentTo("Testing Job Title");
//            dto.CompanyName.Should().BeEquivalentTo("Company");
//            dto.EmployerContact.Should().BeEquivalentTo("Employer Contact");
//            dto.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");
//            dto.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
//            dto.PlacementsKnown.Should().BeTrue();
//            dto.Placements.Should().Be(3);
//            dto.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
//        }
//    }
//}