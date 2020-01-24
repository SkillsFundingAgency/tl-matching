using System;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Opportunity
{
    public class When_OpportunityService_CreateOpportunity_Is_Called : IClassFixture<OpportunityTestFixture>
    {
        private readonly OpportunityTestFixture _testFixture;
        private const string EmployerContact = "Contact for Opportunity Test";
        private readonly int _opportunityId;

        public When_OpportunityService_CreateOpportunity_Is_Called(OpportunityTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testFixture.ResetData(EmployerContact);

            var opportunityDto = new OpportunityDto
            {
                EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                PrimaryContact = EmployerContact
            };

            _opportunityId = _testFixture.OpportunityService.CreateOpportunityAsync(opportunityDto).GetAwaiter().GetResult();
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