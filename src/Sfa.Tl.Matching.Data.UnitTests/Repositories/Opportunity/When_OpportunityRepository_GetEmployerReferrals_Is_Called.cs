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
//    public class When_OpportunityRepository_GetEmployerReferrals_Is_Called
//    {
//        private readonly EmployerReferralDto _result;

//        public When_OpportunityRepository_GetEmployerReferrals_Is_Called()
//        {
//            var logger = Substitute.For<ILogger<OpportunityRepository>>();

//            using (var dbContext = InMemoryDbContext.Create())
//            {
//                dbContext.Add(new ValidOpportunityReferralBuilder().Build());
//                dbContext.AddRange(new ValidOpportunityReferralBuilder().BuildQualificationRouteMapping());
//                dbContext.SaveChanges();

//                var repository = new OpportunityRepository(logger, dbContext);
//                _result = repository.GetEmployerReferrals(1)
//                    .GetAwaiter().GetResult();
//            }
//        }

//        [Fact]
//        public void Then_EmployerReferralDto_Fields_Are_As_Expected()
//        {
//            _result.OpportunityId.Should().Be(1);
//            _result.ProviderReferralInfo.First().ReferralId.Should().Be(1);
//            _result.CompanyName.Should().BeEquivalentTo("Company");
//            _result.EmployerContact.Should().BeEquivalentTo("Employer Contact");
//            _result.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");
//            _result.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
//            _result.JobRole.Should().BeEquivalentTo("Testing Job Title");
//            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");
//            _result.PlacementsKnown.Should().BeTrue();
//            _result.Placements.Should().Be(3);
//            _result.RouteName.Should().BeEquivalentTo("Test Route");
//            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
//            _result.ProviderReferralInfo.Count().Should().Be(1);
//            _result.ProviderReferralInfo.First().ProviderName.Should().BeEquivalentTo("ProviderName");
//            _result.ProviderReferralInfo.First().ProviderPrimaryContact.Should().BeEquivalentTo("PrimaryContact");
//            _result.ProviderReferralInfo.First().ProviderPrimaryContactEmail.Should()
//                .BeEquivalentTo("primary@contact.co.uk");
//            _result.ProviderReferralInfo.First().ProviderPrimaryContactPhone.Should().BeEquivalentTo("01777757777");
//            _result.ProviderReferralInfo.First().ProviderVenuePostcode.Should().BeEquivalentTo("AA1 1AA");
//        }

//        [Fact]
//        public void Then_Two_QualificationShortTitles_Item_Are_Returned() =>
//            _result.ProviderReferralInfo.First()
//                .QualificationShortTitles
//                .Count()
//                .Should().Be(2);

//        [Fact]
//        public void Then_QualificationShortTitles_Includes_First_ShortTitle() =>
//            _result.ProviderReferralInfo.First()
//                .QualificationShortTitles
//                .Should()
//                .Contain("Short Title");

//        [Fact]
//        public void Then_QualificationShortTitles_Includes_Second_ShortTitle() =>
//            _result.ProviderReferralInfo.First()
//                .QualificationShortTitles
//                .Should()
//                .Contain("Duplicate Short Title");
//    }
//}