using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SearchProvider_Post_Is_Called_With_Provider_In_UkRlp : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        public When_Provider_Controller_SearchProvider_Post_Is_Called_With_Provider_In_UkRlp()
        {
            _fixture = new ProviderControllerFixture();

            _fixture.ProviderService
                .SearchAsync(Arg.Any<long>())
                .ReturnsNull();

            _fixture.ProviderService
                .SearchReferenceDataAsync(Arg.Any<long>())
                .Returns(new ProviderSearchResultDto
                {
                    Id = 1,
                    UkPrn = 10000546,
                    Name = "Test Provider"
                });

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            var viewModel = new ProviderSearchParametersViewModel { UkPrn = 10000546 };
            _result = controllerWithClaims.SearchProviderByUkPrnAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_SearchAsync_Is_Called_Exactly_Once()
        {
            _fixture.ProviderService
                .Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_SearchProvidersWithFundingAsync_Is_Not_Called()
        {
            _fixture.ProviderService
                .DidNotReceive()
                .SearchProvidersWithFundingAsync(Arg.Any<ProviderSearchParametersViewModel>());
        }

        [Fact]
        public void Then_ProviderService_SearchReferenceDataAsync_Is_Called_Exactly_Once()
        {
            _fixture.ProviderService
                .Received(1)
                .SearchReferenceDataAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_IsUkRlp_Is_True()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<ProviderSearchViewModel>();
            viewModel.SearchResults.IsUkRlp.Should().BeTrue();
        }
    }
}