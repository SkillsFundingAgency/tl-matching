using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Details_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityDto _dto = new OpportunityDto();
        private const string Contact = "Contact";
        private const string ContactPhone = "ContactPhone";
        private const string ContactEmail = "ContactEmail";
        private const string ModifiedBy = "ModifiedBy";
        private readonly EmployerDetailsViewModel _viewModel = new EmployerDetailsViewModel();

        private const int OpportunityId = 1;

        public When_Employer_Details_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.Contact = Contact;
            _viewModel.ContactEmail = ContactEmail;
            _viewModel.ContactPhone = ContactPhone;

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(_dto);

            var employerController = new EmployerController(null, _opportunityService);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName(ModifiedBy)
                .Build();

            controllerWithClaims.Details(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(_dto);
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
    }
}