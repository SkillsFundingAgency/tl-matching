using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_Model_Errors
    {
        private readonly IActionResult _result;
        private readonly ProviderController _providerController;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_Model_Errors()
        {
            _providerService = Substitute.For<IProviderService>();

            _providerController = new ProviderController(_providerService, new MatchingConfiguration());

            _result = _providerController.SaveProviderDetail(new ProviderDetailViewModel()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_ProviderService_Is_Not_Called() =>
            _providerService.DidNotReceiveWithAnyArgs();

        //[Fact]
        //public void Then_Model_Is_Not_Null()
        //{
        //    var viewResult = _result as ViewResult;
        //    viewResult?.Model.Should().NotBeNull();
        //}

        //[Fact]
        //public void Then_Model_Is_Of_Type_ProviderDetailViewModel()
        //{
        //    var viewResult = _result as ViewResult;
        //    viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();
        //}

        //[Fact]
        //public void Then_ModelState_Is_Not_Valid()
        //{
        //    _providerController.ViewData.ModelState.IsValid.Should().BeFalse();
        //}

        //[Fact]
        //public void Then_ModelState_Contains_Errors()
        //{
        //    _providerController.ViewData.ModelState["DisplayName"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a provider name that is 400 characters or fewer");
        //    _providerController.ViewData.ModelState["PrimaryContact"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must tell us who the primary contact is for industry placements");
        //    _providerController.ViewData.ModelState["PrimaryContactEmail"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter an email for the primary contact");
        //    _providerController.ViewData.ModelState["PrimaryContactPhone"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a telephone number for the primary contact number");
        //    _providerController.ViewData.ModelState["IsEnabledForReferral"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must tell us whether the provider should receive referrals");
        //}
    }
}