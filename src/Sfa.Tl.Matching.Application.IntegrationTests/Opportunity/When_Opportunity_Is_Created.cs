using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Opportunity
{
    public class When_Opportunity_Is_Created : IClassFixture<OpportunityTestFixture>
    {
        private readonly OpportunityTestFixture _testFixture;
        private const string Postcode = "OP1 1CR";
        private const string EmployerContact = "Contact for Opportunity Test";
        private readonly int _opportunityId;

        public When_Opportunity_Is_Created(OpportunityTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testFixture.ResetData(EmployerContact);

            var opportunityDto = new OpportunityDto
            {
                RouteId = 1,
                Postcode = Postcode,
                SearchRadius = 4,
                EmployerId = 5,
                EmployerContact = EmployerContact,
            };

            _opportunityId = _testFixture.OpportunityService.CreateOpportunity(opportunityDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Record_Is_Saved()
        {
            _opportunityId.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Then_Record_Does_Exist() =>
            _testFixture.GetCountBy(EmployerContact).Should().Be(1);
    }
}