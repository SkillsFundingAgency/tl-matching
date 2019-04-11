using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Index_Post_Is_Called_With_Non_Existent_UkPrn
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_Index_Post_Is_Called_With_Non_Existent_UkPrn()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService
                .SearchAsync(Arg.Any<long>())
                .Returns((ProviderSearchResultDto)null);

            var providerController = new ProviderController(_providerService);

            var viewModel = new ProviderSearchParametersViewModel { UkPrn = 12345467 };
            _result = providerController.Index(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerService.Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["UkPrn"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real UKPRN");
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();
    }
}