using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Recording_Referrals_And_Employer_Details_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string Contact = "Contact";
        private const string ContactPhone = "123456789";
        private const string ContactEmail = "ContactEmail";
        private const string ModifiedBy = "ModifiedBy";
        private readonly EmployerDetailDto _dto = new EmployerDetailDto
        {
            OpportunityId = OpportunityId,
            EmployerContact = Contact,
            EmployerContactEmail = ContactEmail,
            EmployerContactPhone = ContactPhone,
            ModifiedBy = ModifiedBy
        };

        private readonly EmployerDetailsViewModel _viewModel = new EmployerDetailsViewModel
        {
            OpportunityId = OpportunityId,
            Contact = Contact,
            ContactEmail = ContactEmail,
            ContactPhone = ContactPhone
        };

        private const int OpportunityId = 1;

        private readonly IActionResult _result;

        public When_Recording_Referrals_And_Employer_Details_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityWithReferrals(OpportunityId).Returns(new OpportunityDto { IsReferral = true });

            var employerController = new EmployerController(null, _opportunityService);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName(ModifiedBy)
                .Build();

            _result = controllerWithClaims.Details(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunityWithReferrals(OpportunityId);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).SaveEmployerDetail(Arg.Is<EmployerDetailDto>(a => 
                a.EmployerContact == Contact && 
                a.EmployerContactEmail == ContactEmail &&
                a.EmployerContactPhone == ContactPhone &&
                a.ModifiedBy == ModifiedBy));
        }

        [Fact]
        public void Then_Contact_Is_Populated()
        {
            _dto.EmployerContact.Should().Be(Contact);
        }

        [Fact]
        public void Then_ContactEmail_Is_Populated()
        {
            _dto.EmployerContactEmail.Should().Be(ContactEmail);
        }

        [Fact]
        public void Then_ContactPhone_Is_Populated()
        {
            _dto.EmployerContactPhone.Should().Be(ContactPhone);
        }

        [Fact]
        public void Then_ModifiedBy_Is_Populated()
        {
            _dto.ModifiedBy.Should().Be(ModifiedBy);
        }

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("CheckAnswersReferrals_Get");
        }
    }
}