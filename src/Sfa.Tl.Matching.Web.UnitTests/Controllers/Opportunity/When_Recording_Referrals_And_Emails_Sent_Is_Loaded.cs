using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_Referrals_And_Emails_Sent_Is_Loaded : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        

        public When_Recording_Referrals_And_Emails_Sent_Is_Loaded(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var dto = new ValidOpportunityDtoBuilder().Build();
            _fixture.EmployerCrmId = dto.EmployerCrmId;

            _fixture.OpportunityService.GetOpportunityAsync(_fixture.OpportunityId).Returns(dto);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.GetReferralEmailSentAsync(_fixture.OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(1).GetOpportunityAsync(_fixture.OpportunityId);
        }

        [Fact]
        public void Then_ViewModel_Properties_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SentViewModel>();
            viewModel.PrimaryContact.Should().Be("EmployerContact");
            viewModel.CompanyName.Should().Be("CompanyName");
            viewModel.EmployerCrmRecord.Should().Be($"https://esfa-cs-prod.crm4.dynamics.com/main.aspx?pagetype=entityrecord&etc=1&id=%7b{_fixture.EmployerCrmId}%7d&extraqs=&newWindow=true");
        }
    }
}