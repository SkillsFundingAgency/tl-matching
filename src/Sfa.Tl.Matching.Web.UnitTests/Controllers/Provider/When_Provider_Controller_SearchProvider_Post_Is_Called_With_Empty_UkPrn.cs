using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SearchProvider_Post_Is_Called_With_Empty_UkPrn : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        public When_Provider_Controller_SearchProvider_Post_Is_Called_With_Empty_UkPrn()
        {
            _fixture = new ProviderControllerFixture();
            
            _fixture.ProviderService
                .SearchAsync(Arg.Any<long>())
                .ReturnsNull();

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            var viewModel = new ProviderSearchParametersViewModel();
            _result = controllerWithClaims.SearchProviderByUkPrnAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_GetSingleOrDefault_Is_Not_Called_Exactly_Once()
        {
            _fixture.ProviderService
                .DidNotReceive()
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_Model_Contains_UkPrn_Error()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ProviderSearchViewModel>();
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["UkPrn"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a UKPRN");
        }
    }
}